using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Legal.GetRulingList
{
    public sealed class GetRulingListQueryHandler
        : IQueryHandler<GetRulingListQuery, List<GetRulingListResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetRulingListQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetRulingListResponse>>> Handle(
            GetRulingListQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    cr.Id,
                    cr.CaseNumber,
                    cr.Year,
                    cr.EmployeeName,
                    cr.Summary,
                    cr.ExecutionType,
                    cr.AttachmentPath,
                    cr.Status,
                    cr.DecisionId,
                    cr.CreatedAt
                FROM HR.CourtRulings cr
                ORDER BY cr.CreatedAt DESC
                """;

            var response = (await connection.QueryAsync<GetRulingListResponse>(sql)).ToList();

            return Result<List<GetRulingListResponse>>.Success(response);
        }
    }
}
