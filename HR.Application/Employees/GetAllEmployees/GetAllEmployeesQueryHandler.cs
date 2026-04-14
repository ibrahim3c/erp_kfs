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
                    Id AS Id,
                    Name AS Name,
                    Code AS Code,
                    Email AS Email,
                    Phone AS Phone,
                    HireDate AS HireDate,
                    IsActive AS IsActive
                FROM 
                    Employees
                ORDER BY 
                    CreatedAt DESC
            """;

            var response = await connection.QueryAsync<EmployeeListResponse>(sql);

            return Result<IEnumerable<EmployeeListResponse>>.Success(response);
        }
    }
}
