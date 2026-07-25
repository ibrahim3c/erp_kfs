using Dapper;
using HR.Domain.Secondments;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Query.GetSecondmentDetails
{
    public class GetSecondmentDetailsHandler : IQueryHandler<GetSecondmentDetailsQuery, SecondmentDetailsDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetSecondmentDetailsHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<SecondmentDetailsDto>> Handle(GetSecondmentDetailsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string headerSql = """
            SELECT s.Id, s.EmployeeId, e.Name AS EmployeeName, jt.Name AS JobTitle,
                   s.Type, s.HostEntityName, s.StartDate, s.EndDate,
                   s.SalaryBearer, s.IncentiveBearer, s.ClearanceCompleted, s.Status, s.FilePath

            FROM HR.Secondments s
            JOIN HR.Employees e ON e.Id = s.EmployeeId
            LEFT JOIN Organization.JobTitles jt ON jt.Id = e.JobTitleId
            WHERE s.Id = @SecondmentId
            """;


            var header = await connection.QuerySingleOrDefaultAsync<SecondmentDetailsDto>( headerSql, new { request.SecondmentId });
            if (header is null)
                return Result<SecondmentDetailsDto>.Failure(SecondmentErrors.NotFound);


            var dto = new SecondmentDetailsDto(
                header.Id, header.EmployeeId, header.EmployeeName, header.JobTitle ?? "-",
                header.Type, header.HostEntityName, header.StartDate, header.EndDate,
                header.SalaryBearer, header.IncentiveBearer, header.ClearanceCompleted,
                header.Status, header.FilePath);

            return Result<SecondmentDetailsDto>.Success(dto);
        }
    }
}
  