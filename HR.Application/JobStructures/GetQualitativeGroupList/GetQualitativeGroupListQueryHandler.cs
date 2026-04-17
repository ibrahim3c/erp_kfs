using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;


namespace HR.Application.JobStructures.GetQualitativeGroupList
{
    public sealed class GetQualitativeGroupListQueryHandler
         : IQueryHandler<GetQualitativeGroupListQuery, List<GetQualitativeGroupListResponse>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetQualitativeGroupListQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<List<GetQualitativeGroupListResponse>>> Handle(
            GetQualitativeGroupListQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = """
                SELECT "Id", "Code", "Name"
                FROM "HR"."QualitativeGroups"
                WHERE "IsActive" = true
                ORDER BY "Name";
                """;

            var result = (await connection.QueryAsync<GetQualitativeGroupListResponse>(sql)).ToList();
            return Result<List<GetQualitativeGroupListResponse>>.Success(result);
        }
    }
}
