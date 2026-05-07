using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Attendance.Queries.GetAbsenceReport
{
    public sealed class GetAbsenceReportQueryHandler
        : IQueryHandler<GetAbsenceReportQuery, AbsenceReportResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetAbsenceReportQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<AbsenceReportResponse>> Handle(
            GetAbsenceReportQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var dateFrom = request.DateFrom.Date;
            var dateTo = request.DateTo.Date;

            var sql = @"
                SELECT
                    e.Id AS EmployeeId,
                    e.Name AS EmployeeName,
                    COALESCE(jt.Name, N'') AS JobTitleName,
                    COALESCE(ou.Name, N'غير محدد') AS DepartmentName,
                    COUNT(ar.Id) AS AbsenceDays
                FROM HR.Employees e
                LEFT JOIN Organization.JobTitles jt ON jt.Id = e.JobTitleId
                LEFT JOIN Organization.OrgUnits ou ON ou.Id = e.OrgUnitId
                LEFT JOIN HR.AttendanceRecords ar
                    ON e.Id = ar.EmployeeId
                    AND ar.Date BETWEEN @DateFrom AND @DateTo
                    AND (ar.Status IS NULL OR ar.Status = 3)
                WHERE e.IsActive = 1";

            var parameters = new DynamicParameters();
            parameters.Add("DateFrom", dateFrom);
            parameters.Add("DateTo", dateTo);

            if (request.OrgUnitId.HasValue && request.OrgUnitId.Value != Guid.Empty)
            {
                sql += " AND e.OrgUnitId = @OrgUnitId";
                parameters.Add("OrgUnitId", request.OrgUnitId.Value);
            }

            sql += @"
                GROUP BY e.Id, e.Name, jt.Name, ou.Name
                HAVING COUNT(ar.Id) > 0
                ORDER BY AbsenceDays DESC, e.Name";

            var items = (await connection.QueryAsync<AbsenceReportItemDto>(sql, parameters)).ToList();

            // Get absent date details per employee
            var employeeIds = items.Select(i => i.EmployeeId).ToList();
            if (employeeIds.Count > 0)
            {
                var datesSql = @"
                    SELECT e.Id AS EmployeeId,
                           CONVERT(NVARCHAR(10), ar.Date, 111) AS AbsentDate
                    FROM HR.AttendanceRecords ar
                    JOIN HR.Employees e ON e.Id = ar.EmployeeId
                    WHERE ar.Date BETWEEN @DateFrom AND @DateTo
                      AND (ar.Status IS NULL OR ar.Status = 3)";

                if (request.OrgUnitId.HasValue && request.OrgUnitId.Value != Guid.Empty)
                {
                    datesSql += " AND e.OrgUnitId = @OrgUnitId";
                }

                var dateRecords = (await connection.QueryAsync<EmployeeDateRecord>(datesSql, parameters)).ToList();

                foreach (var item in items)
                {
                    item.AbsentDates = dateRecords
                        .Where(d => d.EmployeeId == item.EmployeeId)
                        .Select(d => d.AbsentDate)
                        .ToList();
                }
            }

            var response = new AbsenceReportResponse
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                TotalAbsenceDays = items.Sum(i => i.AbsenceDays),
                AffectedEmployeesCount = items.Count,
                Items = items
            };

            return Result<AbsenceReportResponse>.Success(response);
        }

        private class EmployeeDateRecord
        {
            public Guid EmployeeId { get; init; }
            public string AbsentDate { get; init; } = string.Empty;
        }
    }
}
