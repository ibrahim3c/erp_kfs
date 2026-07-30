using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Legal.GetRulingStats
{
    public sealed class GetRulingStatsQueryHandler
        : IQueryHandler<GetRulingStatsQuery, GetRulingStatsResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetRulingStatsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetRulingStatsResponse>> Handle(
            GetRulingStatsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    ISNULL(SUM(CASE WHEN Status = 'NotExecuted' THEN 1 ELSE 0 END), 0) AS PendingCount,
                    ISNULL(SUM(CASE WHEN Status = 'Executed' THEN 1 ELSE 0 END), 0) AS ExecutedCount,
                    ISNULL(SUM(CASE WHEN Status = 'InProgress' THEN 1 ELSE 0 END), 0) AS InProgressCount,
                    COUNT(*) AS TotalCount
                FROM HR.CourtRulings
                """;

            var response = await connection.QueryFirstOrDefaultAsync<GetRulingStatsResponse>(sql)
                ?? new GetRulingStatsResponse();

            return Result<GetRulingStatsResponse>.Success(response);
        }
    }
}
