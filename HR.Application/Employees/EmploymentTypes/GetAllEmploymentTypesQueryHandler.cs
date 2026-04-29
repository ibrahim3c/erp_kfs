using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.EmploymentTypes
{
    public sealed class GetAllEmploymentTypesQueryHandler
        : IQueryHandler<GetAllEmploymentTypesQuery, IEnumerable<EmploymentTypeDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAllEmploymentTypesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<IEnumerable<EmploymentTypeDto>>> Handle(
            GetAllEmploymentTypesQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT Id, Name, Code
                FROM HR.EmploymentTypes
                WHERE IsActive = 1
                ORDER BY Name
            """;

            var result = await connection.QueryAsync<EmploymentTypeDto>(sql);
            return Result<IEnumerable<EmploymentTypeDto>>.Success(result);
        }
    }
}
