
using Dapper;
using HR.Application.Employees.GetAllEmployees;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Employees.GetAllQualificationTypes
{
    public sealed class GetAllQualificationTypesQueryHandler : IQueryHandler<GetAllQualificationTypesQuery, IEnumerable<GetAllQualificationTypesResponse>>
    {
        private readonly ISqlConnectionFactory sqlConnectionFactory;

        public GetAllQualificationTypesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<Result<IEnumerable<GetAllQualificationTypesResponse>>> Handle(GetAllQualificationTypesQuery request, CancellationToken cancellationToken)
        {
            using var connection = sqlConnectionFactory.CreateConnection();
            const string sql = """
                SELECT Id, Name, Description, IsActive FROM HR.QualificationTypes ORDER BY Name
            """;
            var response = await connection.QueryAsync<GetAllQualificationTypesResponse>(sql);
            return Result< IEnumerable<GetAllQualificationTypesResponse>>.Success(response);
        }
    }
}
