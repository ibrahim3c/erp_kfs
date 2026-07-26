using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Decisions.GetDecisionList
{
    public sealed class GetDecisionListQueryHandler
        : IQueryHandler<GetDecisionListQuery, List<GetDecisionListResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetDecisionListQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetDecisionListResponse>>> Handle(
            GetDecisionListQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    d.Id,
                    d.Number,
                    d.Subject,
                    dt.Description AS TypeName,
                    d.DecisionDate,
                    d.Status,
                    d.FilePath,
                    ISNULL(ed.EmployeeCount, 0) AS EmployeeCount
                FROM HR.Decisions d
                LEFT JOIN HR.DecisionTypes dt ON dt.Id = d.DecisionTypeId
                LEFT JOIN (
                    SELECT DecisionId, COUNT(*) AS EmployeeCount
                    FROM HR.EmployeeDecisions
                    GROUP BY DecisionId
                ) ed ON ed.DecisionId = d.Id
                ORDER BY d.DecisionDate DESC
                """;

            var response = (await connection.QueryAsync<GetDecisionListResponse>(sql)).ToList();

            return Result<List<GetDecisionListResponse>>.Success(response);
        }
    }
}
