using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Query.GetEmployeesForSelect
{
    public class GetEmployeesForSelectHandler : IQueryHandler<GetEmployeesForSelectQuery, List<EmployeeSelectDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetEmployeesForSelectHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<List<EmployeeSelectDto>>> Handle(GetEmployeesForSelectQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT e.Id, e.Name, jt.Name AS JobTitle, d.Name AS DepartmentName
            FROM HR.Employees e
            LEFT JOIN Organization.JobTitles jt ON jt.Id = e.JobTitleId
            LEFT JOIN Organization.OrgUnits d ON d.Id = e.OrgUnitId
            WHERE e.IsActive = 1
              AND (@Search IS NULL OR e.Name LIKE '%' + @Search + '%')
            ORDER BY e.Name
            """;

            var data = await connection.QueryAsync<EmployeeSelectDto>(sql, new { request.Search });
            return Result<List<EmployeeSelectDto>>.Success(data.ToList());
        }
    }
}
