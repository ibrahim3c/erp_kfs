using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Funds.GetFundStats
{
    public sealed class GetFundStatsQueryHandler
        : IQueryHandler<GetFundStatsQuery, GetFundStatsResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFundStatsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetFundStatsResponse>> Handle(
            GetFundStatsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    COUNT(*) AS TotalSubscribers,
                    ISNULL(SUM(DeductionAmount), 0) AS MonthlySubscriptionTotal,
                    (SELECT COUNT(*)
                     FROM HR.FundClaims fc
                     WHERE fc.Status IN ('Pending', 'UnderReview')
                    ) AS PendingClaimsCount
                FROM HR.FundSubscriptions fs
                WHERE fs.Status = 'Active'
                """;

            var response = await connection.QueryFirstOrDefaultAsync<GetFundStatsResponse>(sql)
                ?? new GetFundStatsResponse();

            return Result<GetFundStatsResponse>.Success(response);
        }
    }
}
