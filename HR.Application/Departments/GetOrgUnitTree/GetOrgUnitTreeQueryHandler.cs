using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Departments.GetOrgUnitTree
{
    public sealed class GetOrgUnitTreeQueryHandler
        : IQueryHandler<GetOrgUnitTreeQuery, List<GetOrgUnitTreeResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetOrgUnitTreeQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetOrgUnitTreeResponse>>> Handle(
            GetOrgUnitTreeQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    ou.Id,
                    ou.Name,
                    ou.Code,
                    ou.ParentId,
                    parent.Name AS ParentName,
                    ou.OrgUnitTypeId,
                    outh.Name AS OrgUnitTypeName,
                    outh.LevelOrder,
                    ou.IsActive,
                    emp.Name AS CurrentManagerName,
                    ISNULL(ec.EmployeeCount, 0) AS EmployeeCount
                FROM Organization.OrgUnits ou
                LEFT JOIN Organization.OrgUnitTypes outh ON outh.Id = ou.OrgUnitTypeId
                LEFT JOIN Organization.OrgUnits parent ON parent.Id = ou.ParentId
                LEFT JOIN Organization.LeadershipPositions lp ON lp.OrgUnitId = ou.Id
                LEFT JOIN Organization.LeadershipPositionHistories lph
                    ON lph.LeadershipPositionId = lp.Id AND lph.EndDate IS NULL
                LEFT JOIN HR.Employees emp ON emp.Id = lph.EmployeeId
                LEFT JOIN (
                    SELECT OrgUnitId, COUNT(*) AS EmployeeCount
                    FROM HR.Employees
                    WHERE IsActive = 1
                    GROUP BY OrgUnitId
                ) ec ON ec.OrgUnitId = ou.Id
                WHERE ou.IsActive = 1
                ORDER BY outh.LevelOrder, ou.Name
                """;

            var response = (await connection.QueryAsync<GetOrgUnitTreeResponse>(sql)).ToList();

            return Result<List<GetOrgUnitTreeResponse>>.Success(response);
        }
    }
}
