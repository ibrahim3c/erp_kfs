using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Query.List
{
    public class GetTerminationsHandler : IQueryHandler<GetTerminationsQuery, TerminationsResult>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetTerminationsHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<TerminationsResult>> Handle(GetTerminationsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string listSql = """
            SELECT t.Id, t.DecisionNumber, e.Name AS EmployeeName, t.Reason,
                   t.DecisionDate, t.LastWorkingDay, t.AttachmentPath, t.Status
            FROM HR.TerminationDecisions t
            JOIN HR.Employees e ON e.Id = t.EmployeeId
            ORDER BY t.DecisionDate DESC
            """;

            const string countsSql = """
            SELECT Reason, COUNT(*) AS Total
            FROM HR.TerminationDecisions
            WHERE Status = 'Executed'
            GROUP BY Reason
            """;

            var decisions = (await connection.QueryAsync<TerminationListItemDto>(listSql)).ToList();
            var counts = (await connection.QueryAsync<(string Reason, int Total)>(countsSql))
                .ToDictionary(x => x.Reason, x => x.Total);

            var result = new TerminationsResult(
                decisions,
                counts.GetValueOrDefault("Resignation"),
                counts.GetValueOrDefault("Dismissal"),
                counts.GetValueOrDefault("Absence"),
                counts.GetValueOrDefault("Death"));

            return Result<TerminationsResult>.Success(result);
        }
    }
}
