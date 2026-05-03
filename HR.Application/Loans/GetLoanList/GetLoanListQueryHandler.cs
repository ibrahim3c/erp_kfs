using Dapper;
using HR.Domain;
using HR.Domain.Loans;
using Microsoft.AspNetCore.Connections;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.GetLoanList
{
    public class GetLoanListQueryHandler : IQueryHandler<GetLoanListQuery, List<GetLoanListResponse>>
    {
        private readonly IHRUnitOfWork unitOfWork;
        private readonly ISqlConnectionFactory sqlConnectionFactory;

        public GetLoanListQueryHandler(IHRUnitOfWork _unitOfWork,ISqlConnectionFactory _sqlConnectionFactory)
        {
            unitOfWork = _unitOfWork;
            sqlConnectionFactory = _sqlConnectionFactory;
        }
        public async Task<Result<List<GetLoanListResponse>>> Handle(GetLoanListQuery request, CancellationToken cancellationToken)
        {
            /*  Using EF Core
            var loans = await unitOfWork.LoanRepository.GetAllWithEmployeeAsync(cancellationToken);
            var response = loans.Select(loan => new GetLoanListResponse(
                    loan.Id,
                    loan.Employee.Name,
                    loan.StartDate,
                    loan.Amount,
                    loan.Months,
                    loan.InstallmentAmount,
                    loan.RemainingAmount,
                    loan.IsCompleted
                )).ToList();
            */

            // Using Dapper
            using var connection = sqlConnectionFactory.CreateConnection();
            var sql = """
                SELECT 
                    l.Id,
                    e.Name AS EmployeeName, 
                    l.StartDate,
                    l.Amount,
                    l.Months,
                    l.InstallmentAmount,
                    l.RemainingAmount,
                    l.IsCompleted
                FROM HR.Loans l
                INNER JOIN HR.Employees e ON l.EmployeeId = e.Id
                ORDER BY l.StartDate DESC
            """;

            var loans = (await connection.QueryAsync<GetLoanListResponse>(sql)).ToList();
  

            return Result<List<GetLoanListResponse>>.Success(loans);
        }
    }
}
