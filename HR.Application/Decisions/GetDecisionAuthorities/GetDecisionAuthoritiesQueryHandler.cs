using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Decisions.GetDecisionAuthorities
{
    public sealed class GetDecisionAuthoritiesQueryHandler
        : IQueryHandler<GetDecisionAuthoritiesQuery, List<GetDecisionAuthorityResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetDecisionAuthoritiesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetDecisionAuthorityResponse>>> Handle(
            GetDecisionAuthoritiesQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    Id,
                    Name,
                    Description
                FROM HR.DecisionAuthorities
                WHERE IsActive = 1
                ORDER BY Name
                """;

            var response = (await connection.QueryAsync<GetDecisionAuthorityResponse>(sql)).ToList();

            return Result<List<GetDecisionAuthorityResponse>>.Success(response);
        }
    }
}
