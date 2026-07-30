using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Absence.GetAbsenceSettlementStats
{
    public sealed class GetAbsenceSettlementStatsQueryHandler
        : IQueryHandler<GetAbsenceSettlementStatsQuery, AbsenceSettlementStatsResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetAbsenceSettlementStatsQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<AbsenceSettlementStatsResponse>> Handle(
            GetAbsenceSettlementStatsQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var dateFrom = new DateTime(request.Year, request.Month, 1);
            var dateTo = dateFrom.AddMonths(1).AddDays(-1);
            var yearStart = new DateTime(request.Year, 1, 1);

            const string unsettledSql = """
                SELECT COUNT(DISTINCT ar.EmployeeId)
                FROM HR.AttendanceRecords ar
                INNER JOIN HR.Employees e ON e.Id = ar.EmployeeId
                WHERE ar.Date BETWEEN @DateFrom AND @DateTo
                  AND ar.Status = 3
                  AND e.IsActive = 1
                """;

            var unsettledCount = await connection.ExecuteScalarAsync<int>(
                unsettledSql, new { DateFrom = dateFrom, DateTo = dateTo });

            const string overLimitSql = """
                SELECT COUNT(*) FROM (
                    SELECT ar.EmployeeId, COUNT(*) AS TotalAbsence
                    FROM HR.AttendanceRecords ar
                    INNER JOIN HR.Employees e ON e.Id = ar.EmployeeId
                    WHERE ar.Date BETWEEN @YearStart AND @YearEnd
                      AND ar.Status = 3
                      AND e.IsActive = 1
                    GROUP BY ar.EmployeeId
                    HAVING COUNT(*) >= 10
                ) AS OverLimit
                """;

            var overLimitCount = await connection.ExecuteScalarAsync<int>(
                overLimitSql, new { YearStart = yearStart, YearEnd = DateTime.UtcNow.Date });

            return Result<AbsenceSettlementStatsResponse>.Success(
                new AbsenceSettlementStatsResponse
                {
                    UnsettledCount = unsettledCount,
                    OverLegalLimitCount = overLimitCount
                });
        }
    }
}
