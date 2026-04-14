using Dapper;
using HR.Domain.Employees;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Employees.GetEmployeeDetails
{
    public sealed class GetEmployeeDetailsQueryHandler : IQueryHandler<GetEmployeeDetailsQuery, GetEmployeeDetailsResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetEmployeeDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetEmployeeDetailsResponse>> Handle(GetEmployeeDetailsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            // Note: Adjust the table name ("Employees") and column names if they differ in your actual database schema
            const string sql = """
                SELECT 
                    Id AS Id,
                    Name AS Name,
                    Code AS Code,
                    Email AS Email,
                    Phone AS Phone,
                    HireDate AS HireDate,
                    IsActive AS IsActive,
                    CreatedAt AS CreatedAt
                FROM 
                    Employees
                WHERE 
                    Id = @EmployeeId
            """;

            var response = await connection.QuerySingleOrDefaultAsync<GetEmployeeDetailsResponse>(sql, new { request.EmployeeId });

            if (response is null)
                return Result<GetEmployeeDetailsResponse>.Failure(EmployeeErrors.NotFound);

            return Result<GetEmployeeDetailsResponse>.Success(response);
        }
    }
}
