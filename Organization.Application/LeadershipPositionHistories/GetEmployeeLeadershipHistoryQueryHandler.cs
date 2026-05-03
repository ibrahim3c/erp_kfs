using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;


namespace Organization.Application.LeadershipPositionHistories
{
    public sealed class GetEmployeeLeadershipHistoryQueryHandler
      : IQueryHandler<GetEmployeeLeadershipHistoryQuery, List<EmployeeLeadershipHistoryResponse>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetEmployeeLeadershipHistoryQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<List<EmployeeLeadershipHistoryResponse>>> Handle(
            GetEmployeeLeadershipHistoryQuery request,
            CancellationToken cancellationToken)
        {
            const string sql = """
            SELECT
                h.Id,
                h.StartDate,
                h.EndDate,
                h.DecisionNumber,
                h.DecisionDate,
                h.Notes,
                jt.Name + ' - ' + ou.Name AS PositionName
            FROM Organization.LeadershipPositionHistories h
            INNER JOIN Organization.LeadershipPositions lp ON lp.Id = h.LeadershipPositionId
            INNER JOIN Organization.JobTitles           jt ON jt.Id = lp.JobTitleId
            INNER JOIN Organization.OrgUnits            ou ON ou.Id = lp.OrgUnitId
            WHERE h.EmployeeId = @EmployeeId
            ORDER BY h.StartDate DESC
        """;
            
            using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryAsync<EmployeeLeadershipHistoryResponse>(
                sql,
                new { request.EmployeeId });

            return Result<List<EmployeeLeadershipHistoryResponse>>.Success(result.ToList());
        }
    }
}
