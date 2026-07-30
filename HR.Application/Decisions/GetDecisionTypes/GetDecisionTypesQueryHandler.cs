using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Decisions.GetDecisionTypes
{
    public sealed class GetDecisionTypesQueryHandler
        : IQueryHandler<GetDecisionTypesQuery, List<GetDecisionTypeResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetDecisionTypesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetDecisionTypeResponse>>> Handle(
            GetDecisionTypesQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    Id,
                    Code,
                    Description
                FROM HR.DecisionTypes
                WHERE IsActive = 1
                ORDER BY Code
                """;

            var response = (await connection.QueryAsync<GetDecisionTypeResponse>(sql)).ToList();

            return Result<List<GetDecisionTypeResponse>>.Success(response);
        }
    }
}
