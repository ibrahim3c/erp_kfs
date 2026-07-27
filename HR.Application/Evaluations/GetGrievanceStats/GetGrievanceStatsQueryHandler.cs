using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Evaluations.GetGrievanceStats
{
    public sealed class GetGrievanceStatsQueryHandler
        : IQueryHandler<GetGrievanceStatsQuery, GetGrievanceStatsResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetGrievanceStatsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetGrievanceStatsResponse>> Handle(
            GetGrievanceStatsQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    COUNT(*) AS TotalGrievances,
                    SUM(CASE WHEN Status IN ('Pending', 'UnderReview') THEN 1 ELSE 0 END) AS PendingCount,
                    SUM(CASE WHEN Status = 'Accepted' THEN 1 ELSE 0 END) AS AcceptedCount,
                    SUM(CASE WHEN Status = 'Rejected' THEN 1 ELSE 0 END) AS RejectedCount
                FROM HR.Grievances
                """;

            var response = await connection.QuerySingleOrDefaultAsync<GetGrievanceStatsResponse>(sql);

            response ??= new GetGrievanceStatsResponse();

            return Result<GetGrievanceStatsResponse>.Success(response);
        }
    }
}
