using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Departments.GetDepartmentStats
{
    public sealed class GetDepartmentStatsQueryHandler
        : IQueryHandler<GetDepartmentStatsQuery, GetDepartmentStatsResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetDepartmentStatsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetDepartmentStatsResponse>> Handle(
            GetDepartmentStatsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    COUNT(*) AS TotalUnits,
                    SUM(CASE WHEN ou.IsActive = 1 THEN 1 ELSE 0 END) AS ActiveUnits,
                    SUM(CASE WHEN outh.LevelOrder = 2 THEN 1 ELSE 0 END) AS DepartmentCount,
                    SUM(CASE WHEN outh.LevelOrder = 3 THEN 1 ELSE 0 END) AS SectionCount,
                    ISNULL((SELECT COUNT(*) FROM HR.Employees WHERE IsActive = 1), 0) AS TotalEmployees
                FROM Organization.OrgUnits ou
                LEFT JOIN Organization.OrgUnitTypes outh ON outh.Id = ou.OrgUnitTypeId
                """;

            var response = await connection.QuerySingleOrDefaultAsync<GetDepartmentStatsResponse>(sql);

            response ??= new GetDepartmentStatsResponse();

            return Result<GetDepartmentStatsResponse>.Success(response);
        }
    }
}
