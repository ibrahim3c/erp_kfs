using Dapper;
using HR.Domain.ServiceTerms.Entities;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Query.GetServiceTermDetails
{
    public class GetServiceTermDetailsHandler : IQueryHandler<GetServiceTermDetailsQuery, ServiceTermDetailsDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetServiceTermDetailsHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<ServiceTermDetailsDto>> Handle(GetServiceTermDetailsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT st.Id, e.Name AS EmployeeName, st.PreviousEntityName, st.Type,
                       st.StartDate, st.EndDate, st.Status, st.AdjustedSeniorityDate,
                       st.RejectionReason, st.CommitteeDecisionNumber, st.AttachmentPath
                FROM HR.ServiceTermRecords st
                JOIN HR.Employees e ON e.Id = st.EmployeeId
                WHERE st.Id = @ServiceTermId
                """;

            var dto = await connection.QuerySingleOrDefaultAsync<ServiceTermDetailsDto>(sql, new { request.ServiceTermId });
            if (dto is null)
                return Result<ServiceTermDetailsDto>.Failure(ServiceTermErrors.NotFound);

            return Result<ServiceTermDetailsDto>.Success(dto);
        }
    }
}
