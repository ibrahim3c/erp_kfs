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
                    l."Id" AS LoanId,
                    e."Name" AS EmployeeName, 
                    l."StartDate" AS StartDate,
                    l."Amount" AS Amount,
                    l."Months" AS Months,
                    l."InstallmentAmount" AS InstallmentAmount,
                    l."RemainingAmount" AS RemainingAmount,
                    l."IsCompleted" AS IsCompleted
                FROM 
                    "HR"."Loans" l
                INNER JOIN 
                    "HR"."Employees" e ON l."EmployeeId" = e."Id"
                ORDER BY 
                    l."StartDate" DESC;    
                """;

            var loans = (await connection.QueryAsync<GetLoanListResponse>(sql)).ToList();
            if (loans == null || loans.Count == 0)        
                return Result<List<GetLoanListResponse>>.Failure(LoanErrors.NotFoundLoans);

            return Result<List<GetLoanListResponse>>.Success(loans);
        }
    }
}
