using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Query.GetServiceTerms
{
    public class GetServiceTermsHandler : IQueryHandler<GetServiceTermsQuery, List<ServiceTermListItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetServiceTermsHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<List<ServiceTermListItemDto>>> Handle(GetServiceTermsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT st.Id, st.EmployeeId, e.Name AS EmployeeName, st.PreviousEntityName,
                   st.StartDate, st.EndDate, st.Status, st.AdjustedSeniorityDate, st.AttachmentPath
            FROM HR.ServiceTermRecords st
            JOIN HR.Employees e ON e.Id = st.EmployeeId
            ORDER BY st.CreatedOn DESC
            """;

            var data = await connection.QueryAsync<ServiceTermListItemDto>(sql);
            return Result<List<ServiceTermListItemDto>>.Success(data.ToList());
        }
    }
}
