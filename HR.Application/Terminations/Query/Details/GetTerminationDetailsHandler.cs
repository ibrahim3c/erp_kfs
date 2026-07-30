using Dapper;
using HR.Domain.Terminations;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Query.Details
{
    public class GetTerminationDetailsHandler : IQueryHandler<GetTerminationDetailsQuery, TerminationDetailsDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetTerminationDetailsHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<TerminationDetailsDto>> Handle(GetTerminationDetailsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT t.Id, t.DecisionNumber, e.Name AS EmployeeName,  jt.Name AS JobTitle,
                   t.Reason, t.DecisionDate, t.LastWorkingDay, t.LegalBasis,
                   t.AttachmentPath, t.Status, t.CancellationReason, t.UpdatedOn
            FROM HR.TerminationDecisions t
            JOIN HR.Employees e ON e.Id = t.EmployeeId
            LEFT JOIN Organization.JobTitles jt ON jt.Id = e.JobTitleId
            WHERE t.Id = @TerminationId
            """;

            var details = await connection.QueryFirstOrDefaultAsync<TerminationDetailsDto>(sql, new { request.TerminationId });

            if (details is null)
                return Result<TerminationDetailsDto>.Failure(TerminationErrors.NotFound);

            return Result<TerminationDetailsDto>.Success(details);
        }
    }
}
