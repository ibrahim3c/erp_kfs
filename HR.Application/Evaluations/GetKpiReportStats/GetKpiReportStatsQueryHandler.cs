using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Evaluations.GetKpiReportStats
{
    public sealed class GetKpiReportStatsQueryHandler
        : IQueryHandler<GetKpiReportStatsQuery, GetKpiReportStatsResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetKpiReportStatsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetKpiReportStatsResponse>> Handle(
            GetKpiReportStatsQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var sql = """
                SELECT
                    COUNT(*) AS TotalReports,
                    SUM(CASE WHEN Status = 'Approved' THEN 1 ELSE 0 END) AS ApprovedCount,
                    SUM(CASE WHEN Status = 'Draft' THEN 1 ELSE 0 END) AS DraftCount,
                    ISNULL(AVG(Score), 0) AS AverageScore
                FROM HR.KpiReports
                """;

            if (request.Year.HasValue)
            {
                sql += " WHERE Year = @Year";
            }

            var response = await connection.QuerySingleOrDefaultAsync<GetKpiReportStatsResponse>(
                sql, new { Year = request.Year });

            response ??= new GetKpiReportStatsResponse();

            return Result<GetKpiReportStatsResponse>.Success(response);
        }
    }
}
