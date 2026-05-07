using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Attendance.Queries.GetDailyAttendance
{
    public sealed class GetDailyAttendanceQueryHandler
        : IQueryHandler<GetDailyAttendanceQuery, DailyAttendanceResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetDailyAttendanceQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<DailyAttendanceResponse>> Handle(
            GetDailyAttendanceQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();
            var date = request.Date?.Date ?? DateTime.UtcNow.Date;

            var sql = """
                SELECT
                    e.Id AS EmployeeId,
                    e.Name AS EmployeeName,
                    COALESCE(ou.Name, N'غير محدد') AS DepartmentName,
                    FORMAT(ar.CheckIn, N'hh\:mm') AS CheckIn,
                    FORMAT(ar.CheckOut, N'hh\:mm') AS CheckOut,
                    COALESCE(ar.WorkedHours, 0) AS WorkedHours,
                    COALESCE(ar.LateMinutes, 0) AS LateMinutes,
                    CASE
                        WHEN ar.Status IS NULL THEN N'غياب'
                        WHEN ar.Status = 1 THEN N'حضور منتظم'
                        WHEN ar.Status = 2 THEN N'تأخير'
                        WHEN ar.Status = 3 THEN N'غياب'
                        WHEN ar.Status = 4 THEN N'مأمورية خارجية'
                        WHEN ar.Status = 5 THEN N'إجازة'
                        WHEN ar.Status = 6 THEN N'إذن'
                        ELSE N'غير محدد'
                    END AS Status,
                    CASE
                        WHEN ar.Status IS NULL THEN 'bg-danger'
                        WHEN ar.Status = 1 THEN 'bg-success'
                        WHEN ar.Status = 2 THEN 'bg-warning text-dark'
                        WHEN ar.Status = 3 THEN 'bg-danger'
                        WHEN ar.Status = 4 THEN 'bg-info text-dark'
                        WHEN ar.Status = 5 THEN 'bg-secondary'
                        WHEN ar.Status = 6 THEN 'bg-primary'
                        ELSE 'bg-secondary'
                    END AS StatusClass,
                    ar.Notes
                FROM HR.Employees e
                LEFT JOIN Organization.OrgUnits ou ON e.OrgUnitId = ou.Id
                LEFT JOIN HR.AttendanceRecords ar ON e.Id = ar.EmployeeId AND ar.Date = @Date
                WHERE e.IsActive = 1
                """;

            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();
            parameters.Add("Date", date);

            if (request.OrgUnitId.HasValue && request.OrgUnitId.Value != Guid.Empty)
            {
                whereClauses.Add("e.OrgUnitId = @OrgUnitId");
                parameters.Add("OrgUnitId", request.OrgUnitId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "all")
            {
                switch (request.Status.ToLower())
                {
                    case "present":
                        whereClauses.Add("ar.Status = 1");
                        break;
                    case "late":
                        whereClauses.Add("ar.Status = 2");
                        break;
                    case "absent":
                        whereClauses.Add("(ar.Status IS NULL OR ar.Status = 3)");
                        break;
                    case "mission":
                        whereClauses.Add("ar.Status = 4");
                        break;
                    case "vacation":
                        whereClauses.Add("ar.Status = 5");
                        break;
                    case "permission":
                        whereClauses.Add("ar.Status = 6");
                        break;
                }
            }

            if (whereClauses.Count > 0)
                sql += " AND " + string.Join(" AND ", whereClauses);

            sql += " ORDER BY e.Name";

            var items = (await connection.QueryAsync<AttendanceRowDto>(sql, parameters)).ToList();

            // Stats
            var totalWorkforce = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.Employees WHERE IsActive = 1");

            var presentCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.AttendanceRecords WHERE Date = @Date AND Status = 1", new { Date = date });

            var lateCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.AttendanceRecords WHERE Date = @Date AND Status = 2", new { Date = date });

            var absentCount = totalWorkforce - presentCount - lateCount;

            var response = new DailyAttendanceResponse
            {
                TotalWorkforce = totalWorkforce,
                PresentCount = presentCount,
                LateCount = lateCount,
                AbsentOrVacationCount = absentCount,
                CurrentDate = date,
                Items = items
            };

            return Result<DailyAttendanceResponse>.Success(response);
        }
    }
}
