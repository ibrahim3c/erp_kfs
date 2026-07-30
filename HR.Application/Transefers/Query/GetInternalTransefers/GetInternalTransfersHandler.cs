using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Query.GetInternalTransefers
{
    public class GetInternalTransfersHandler : IQueryHandler<GetInternalTransfersQuery, List<InternalTransferListItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetInternalTransfersHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<List<InternalTransferListItemDto>>> Handle(GetInternalTransfersQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT it.Id, it.EmployeeId, e.Name AS EmployeeName,
                   fromDept.Name AS FromDepartmentName, toDept.Name AS ToDepartmentName, jt.Name AS NewJobTitleName,
                   it.Reason, it.ExecutionDate, it.Status

            FROM HR.InternalTransfers it
            JOIN HR.Employees e ON e.Id = it.EmployeeId
            JOIN Organization.OrgUnits fromDept ON fromDept.Id = it.FromDepartmentId
            JOIN Organization.OrgUnits toDept ON toDept.Id = it.ToDepartmentId
            JOIN Organization.JobTitles jt ON jt.Id = it.NewJobTitleId
            ORDER BY it.CreatedOn DESC
            """;

            var data = await connection.QueryAsync<InternalTransferListItemDto>(sql);
            return Result<List<InternalTransferListItemDto>>.Success(data.ToList());
        }
    }
}
