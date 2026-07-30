using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Query.GetExternalMovements
{
    public class GetExternalMovementsHandler : IQueryHandler<GetExternalMovementsQuery, List<ExternalMovementListItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetExternalMovementsHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<List<ExternalMovementListItemDto>>> Handle(GetExternalMovementsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT em.Id, e.Name AS EmployeeName, em.Type, em.Direction, em.OtherEntityName,
                   em.StartDate, em.EndDate, em.Status, em.AttachmentPath
            FROM HR.ExternalMovements em
            JOIN HR.Employees e ON e.Id = em.EmployeeId
            ORDER BY em.CreatedOn DESC
            """;

            var data = await connection.QueryAsync<ExternalMovementListItemDto>(sql);
            return Result<List<ExternalMovementListItemDto>>.Success(data.ToList());
        }
    }
}
