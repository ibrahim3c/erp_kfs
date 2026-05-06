using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;


namespace HR.Application.Permissions.GetAttendanceLog
{
    public sealed class GetAttendanceLogQueryHandler
        : IQueryHandler<GetAttendanceLogQuery, GetAttendanceLogResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetAttendanceLogQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<GetAttendanceLogResponse>> Handle(
            GetAttendanceLogQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            // ─── Summary ───────────────────────────────────────────

            // Query بيحسب إجمالي الدقائق (Permissions + Late)
            var summarySql = """
                SELECT 
                    COALESCE(SUM(CASE WHEN src = 'P' THEN dur ELSE 0 END), 0) AS TotalPermissionMinutes,
                    COALESCE(SUM(CASE WHEN src = 'L' THEN dur ELSE 0 END), 0) AS TotalLateMinutes
                FROM ( 
                    SELECT 'P' AS src, DurationMinutes AS dur       
                    FROM HR.PermissionRequests 
                    WHERE MONTH(Date) = @Month AND YEAR(Date) = @Year

                    UNION ALL 

                    SELECT 'L' AS src, LateMinutes AS dur           
                    FROM HR.LateEntries 
                    WHERE MONTH(Date) = @Month AND YEAR(Date) = @Year
                ) combined
                """;

            // تنفيذ الـ query وإرجاع أول row (فيه الإجماليات)
            var summary = await connection.QueryFirstAsync(
                summarySql, new { request.Month, request.Year });


            // ─── عدد الموظفين اللي اتعاقبوا 

            var exceededSql = """
                SELECT COUNT(DISTINCT EmployeeId)
                FROM HR.LateEntries
                WHERE MONTH(Date) = @Month 
                  AND YEAR(Date) = @Year 
                  AND IsTransferredToPenalty = 1
                """;

            // تنفيذ query وإرجاع رقم واحد (int)
            var exceeded = await connection.ExecuteScalarAsync<int>(
                exceededSql, new { request.Month, request.Year });


            // ─── Log Items (Permissions + Late مع بعض)

            var logSql = """
                SELECT 
                    pr.Id, pr.Date, e.Name AS EmployeeName,
                    'Permission'            AS Type,
                    pr.PermissionType       AS SubType,
                    FORMAT(pr.FromTime, 'hh\:mm') + ' : ' + FORMAT(pr.ToTime, 'hh\:mm') AS TimeRange,
                    pr.DurationMinutes,
                    pr.Notes,
                    CASE pr.PermissionType 
                        WHEN 'Personal' THEN 'مخصوم من الرصيد'
                        WHEN 'Official' THEN 'عمل رسمي'
                        WHEN 'Medical'  THEN 'إذن مرضي'
                    END AS StatusLabel,
                    CAST(0 AS BIT) AS IsTransferred
                FROM HR.PermissionRequests pr 
                INNER JOIN HR.Employees e ON pr.EmployeeId = e.Id
                WHERE MONTH(pr.Date) = @Month AND YEAR(pr.Date) = @Year

                UNION ALL 

                SELECT 
                    le.Id, le.Date, e.Name AS EmployeeName,
                    'Late'              AS Type,
                    N'تأخير صباحي'     AS SubType,
                    N'حضور ' + FORMAT(le.ActualArrivalTime, 'hh\:mm') AS TimeRange,
                    le.LateMinutes      AS DurationMinutes,
                    le.Notes,
                    CASE le.IsTransferredToPenalty 
                        WHEN 1 THEN N'مرحل للجزاء'
                        ELSE N'قيد التجميع'
                    END AS StatusLabel,
                    le.IsTransferredToPenalty AS IsTransferred
                FROM HR.LateEntries le 
                INNER JOIN HR.Employees e ON le.EmployeeId = e.Id
                WHERE MONTH(le.Date) = @Month AND YEAR(le.Date) = @Year

                ORDER BY Date DESC
                """;

            var items = (await connection.QueryAsync<AttendanceLogItem>(logSql, new { request.Month, request.Year })).ToList();

            var response = new GetAttendanceLogResponse
            {
                TotalPermissionMinutes = (int)summary.TotalPermissionMinutes,
                TotalLateMinutes = (int)summary.TotalLateMinutes,
                EmployeesExceededLimit = exceeded,
                Items = items
            };

            return Result<GetAttendanceLogResponse>.Success(response);
        }
    }
}
