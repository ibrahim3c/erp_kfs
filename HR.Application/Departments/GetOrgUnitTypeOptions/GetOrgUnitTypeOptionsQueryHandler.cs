using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Departments.GetOrgUnitTypeOptions
{
    public sealed class GetOrgUnitTypeOptionsQueryHandler
        : IQueryHandler<GetOrgUnitTypeOptionsQuery, List<GetOrgUnitTypeOptionsResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetOrgUnitTypeOptionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetOrgUnitTypeOptionsResponse>>> Handle(
            GetOrgUnitTypeOptionsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    Id,
                    Name,
                    LevelOrder,
                    CanHaveChild
                FROM Organization.OrgUnitTypes
                ORDER BY LevelOrder
                """;

            var response = (await connection.QueryAsync<GetOrgUnitTypeOptionsResponse>(sql)).ToList();

            return Result<List<GetOrgUnitTypeOptionsResponse>>.Success(response);
        }
    }
}
