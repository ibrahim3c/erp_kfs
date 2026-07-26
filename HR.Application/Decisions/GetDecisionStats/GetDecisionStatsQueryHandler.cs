using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Decisions.GetDecisionStats
{
    public sealed class GetDecisionStatsQueryHandler
        : IQueryHandler<GetDecisionStatsQuery, GetDecisionStatsResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetDecisionStatsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetDecisionStatsResponse>> Handle(
            GetDecisionStatsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    COUNT(*) AS TotalDecisions,
                    SUM(CASE WHEN d.Status = 'Draft' OR d.Status = 'Approved' THEN 1 ELSE 0 END) AS PendingExecution,
                    ISNULL(appt.NewAppointments, 0) AS NewAppointments,
                    ISNULL(prom.Promotions, 0) AS Promotions
                FROM HR.Decisions d
                LEFT JOIN (
                    SELECT COUNT(DISTINCT ed.DecisionId) AS NewAppointments
                    FROM HR.EmployeeDecisions ed
                    INNER JOIN HR.Decisions d2 ON d2.Id = ed.DecisionId
                    INNER JOIN HR.DecisionTypes dt2 ON dt2.Id = d2.DecisionTypeId
                    WHERE dt2.AffectsEmploymentType = 1
                ) appt ON 1 = 1
                LEFT JOIN (
                    SELECT COUNT(DISTINCT ed.DecisionId) AS Promotions
                    FROM HR.EmployeeDecisions ed
                    INNER JOIN HR.Decisions d3 ON d3.Id = ed.DecisionId
                    INNER JOIN HR.DecisionTypes dt3 ON dt3.Id = d3.DecisionTypeId
                    WHERE dt3.AffectsPosition = 1
                ) prom ON 1 = 1
                """;

            var response = await connection.QuerySingleOrDefaultAsync<GetDecisionStatsResponse>(sql);

            response ??= new GetDecisionStatsResponse();

            return Result<GetDecisionStatsResponse>.Success(response);
        }
    }
}
