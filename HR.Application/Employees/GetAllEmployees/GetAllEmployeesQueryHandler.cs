using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.GetAllEmployees
{
    public sealed class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery,IEnumerable<EmployeeListResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAllEmployeesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<IEnumerable<EmployeeListResponse>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    e.Id,
                    e.Code,
                    e.Name,
                    e.IsActive,
                    e.LeadershipPositionId,
                    jt.Name  AS JobTitleName,
                    jg.Name  AS JobGradeName,
                    ou.Name  AS OrgUnitName
                FROM HR.Employees e
                LEFT JOIN Organization.JobTitles       jt ON jt.Id = e.JobTitleId
                LEFT JOIN Organization.JobGrades       jg ON jg.Id = e.JobGradeId
                LEFT JOIN Organization.OrgUnits        ou ON ou.Id = e.OrgUnitId
                WHERE e.IsActive = 1
                ORDER BY e.Code
                """;

            var response = await connection.QueryAsync<EmployeeListResponse>(sql);

            return Result<IEnumerable<EmployeeListResponse>>.Success(response);
        }
    }
}
