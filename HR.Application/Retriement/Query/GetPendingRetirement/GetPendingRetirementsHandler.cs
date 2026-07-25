using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Query.GetPendingRetirement
{
    public class GetPendingRetirementsHandler : IQueryHandler<GetPendingRetirementsQuery, List<PendingRetirementDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetPendingRetirementsHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<Result<List<PendingRetirementDto>>> Handle(GetPendingRetirementsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT
                e.Id            AS EmployeeId,
                e.Name          AS EmployeeName,
                jt.Name         AS JobTitle,
                e.DateOfBirth,
                DATEADD(YEAR, 60, e.DateOfBirth) AS RetirementDate,  
                rf.Id           AS RetirementFileId,
                rf.Stage        AS FileStatus
            FROM HR.Employees e
            LEFT JOIN Organization.JobTitles jt ON jt.Id = e.JobTitleId
            LEFT JOIN HR.RetirementFiles rf ON rf.EmployeeId = e.Id
            WHERE e.IsActive = 1
              AND YEAR(DATEADD(YEAR, 60, e.DateOfBirth)) = @Year
            ORDER BY RetirementDate ASC
            """;

            var data = await connection.QueryAsync<PendingRetirementDto>(sql, new { request.Year });
            return Result<List<PendingRetirementDto>>.Success(data.ToList());
        }
    }
    }

