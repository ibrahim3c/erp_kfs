using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Query.GetDepartmentsForSelect
{
    public class GetDepartmentsForSelectHandler : IQueryHandler<GetDepartmentsForSelectQuery, DepartmentWithJobTitlesDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetDepartmentsForSelectHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<DepartmentWithJobTitlesDto>> Handle(GetDepartmentsForSelectQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var deptQuery = """
                SELECT Id, Name
                FROM Organization.OrgUnits
                ORDER BY Name
                """;

            var jobTitleQuery = """
                SELECT Id, Name
                FROM Organization.JobTitles
                ORDER BY Name
                """;

            var departments = await connection.QueryAsync<DepartmentSelectDto>(deptQuery);
            var jobTitles = await connection.QueryAsync<JobTitleDto>(jobTitleQuery);

            return Result<DepartmentWithJobTitlesDto>.Success(new DepartmentWithJobTitlesDto(departments.ToList(), jobTitles.ToList()));
        }
    }
}
