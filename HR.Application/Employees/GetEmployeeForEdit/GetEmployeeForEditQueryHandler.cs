using Dapper;
using HR.Domain.Employees;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.GetEmployeeForEdit
{
    public sealed class GetEmployeeForEditQueryHandler : IQueryHandler<GetEmployeeForEditQuery, GetEmployeeForEditResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetEmployeeForEditQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetEmployeeForEditResponse>> Handle(GetEmployeeForEditQuery request, CancellationToken cancellationToken)
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
                    IsActive AS IsActive,
                    
                FROM HR.Employees 
                WHERE Id = @EmployeeId
            """;

            var response = await connection.QuerySingleOrDefaultAsync<GetEmployeeForEditResponse>(sql, new { request.EmployeeId });

            if (response is null)
                return Result<GetEmployeeForEditResponse>.Failure(EmployeeErrors.NotFound);

            return Result<GetEmployeeForEditResponse>.Success(response);
        }
    }
}