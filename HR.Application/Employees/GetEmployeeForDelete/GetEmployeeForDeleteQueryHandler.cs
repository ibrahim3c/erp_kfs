using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using Dapper;
using HR.Domain.Employees;
namespace HR.Application.Employees.GetEmployeeForDelete
{
    public sealed class GetEmployeeForDeleteQueryHandler : IQueryHandler<GetEmployeeForDeleteQuery, GetEmployeeForDeleteResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetEmployeeForDeleteQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetEmployeeForDeleteResponse>> Handle(GetEmployeeForDeleteQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT 
                    Id AS Id,
                    Name AS Name,
                    Code AS Code,
                    Email AS Email
                FROM 
                    Employees
                WHERE 
                    Id = @EmployeeId
            """;

            var response = await connection.QuerySingleOrDefaultAsync<GetEmployeeForDeleteResponse>(sql, new { request.EmployeeId });

            if (response is null)
                return Result<GetEmployeeForDeleteResponse>.Failure(EmployeeErrors.NotFound);

            return Result<GetEmployeeForDeleteResponse>.Success(response);
        }
    }
}
