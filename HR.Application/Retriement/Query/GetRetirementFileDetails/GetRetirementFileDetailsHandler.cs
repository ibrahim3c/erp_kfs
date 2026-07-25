using Dapper;
using HR.Domain.Retirement.Entities;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Query.GetRetirementFileDetails
{
    public class GetRetirementFileDetailsHandler : IQueryHandler<GetRetirementFileDetailsQuery, RetirementFileDetailsDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetRetirementFileDetailsHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<RetirementFileDetailsDto>> Handle(GetRetirementFileDetailsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string headerSql = """
            SELECT rf.Id, rf.EmployeeId, e.Name AS EmployeeName, rf.ReferralDate,
                   rf.JoinPeriodsAdded, rf.SpecialLeavesReviewed, rf.Notes
            FROM HR.RetirementFiles rf
            JOIN HR.Employees e ON e.Id = rf.EmployeeId
            WHERE rf.Id = @RetirementFileId
            """;

            const string salarySql = """
            SELECT Year, BasicInsuredSalary
            FROM HR.RetirementSalaryRecords
            WHERE RetirementFileId = @RetirementFileId
            ORDER BY Year
            """;

            var header = await connection.QuerySingleOrDefaultAsync<RetirementFileHeaderDto>(headerSql, new { request.RetirementFileId });
            if (header is null)
                return Result<RetirementFileDetailsDto>.Failure(RetirementErrors.NotFound);

            var salaries = (await connection.QueryAsync<SalaryYearDto>(salarySql, new { request.RetirementFileId })).ToList();

           var dto = new RetirementFileDetailsDto(
           header.Id, header.EmployeeId, header.EmployeeName, header.ReferralDate,
           header.JoinPeriodsAdded, header.SpecialLeavesReviewed, header.Notes, salaries);

            return Result<RetirementFileDetailsDto>.Success(dto);
        }
    }
}
