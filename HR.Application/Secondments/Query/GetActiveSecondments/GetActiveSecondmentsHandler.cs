using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Query.GetActiveSecondments
{
    public class GetActiveSecondmentsHandler : IQueryHandler<GetActiveSecondmentsQuery, List<SecondmentListItemDto>>
    {

        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetActiveSecondmentsHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<List<SecondmentListItemDto>>> Handle(GetActiveSecondmentsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT s.Id, s.EmployeeId, e.Name AS EmployeeName, s.Type,
                   s.HostEntityName, s.StartDate, s.EndDate,
                   s.SalaryBearer, s.Status, s.ClearanceCompleted
            FROM HR.Secondments s
            JOIN HR.Employees e ON e.Id = s.EmployeeId
            WHERE s.Status <> 'Ended'  -- not ended secondments
            ORDER BY s.EndDate ASC
            """;

            var data = await connection.QueryAsync<SecondmentListItemDto>(sql);
            return Result<List<SecondmentListItemDto>>.Success(data.ToList());
        }
    }
}
