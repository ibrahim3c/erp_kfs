using Dapper;
using HR.Application.Employees.GetAllEmployees;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.GetAllEmployeeActiveAndNot
{
    public class GetAllEmployeesActiveAndNotQueryHandler : IQueryHandler<GetAllEmployeesActiveAndNotQuery, IEnumerable<GetAllEmployeesQueryActiveAndNotResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAllEmployeesActiveAndNotQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<Result<IEnumerable<GetAllEmployeesQueryActiveAndNotResponse>>> Handle(GetAllEmployeesActiveAndNotQuery request, CancellationToken cancellationToken)
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
                ORDER BY e.Code
                """;

            var response = await connection.QueryAsync<GetAllEmployeesQueryActiveAndNotResponse>(sql);

            return Result<IEnumerable<GetAllEmployeesQueryActiveAndNotResponse>>.Success(response.ToList());
        }
    }
}
