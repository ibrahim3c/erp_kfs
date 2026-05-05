using Dapper;
using HR.Domain.Loans;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.GetLoanDetails
{
    public class GetLoanQueryHandler : IQueryHandler<GetLoanDetailsQuery, GetLoanDetailsQueryResponse>
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public GetLoanQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }
        public async Task<Result<GetLoanDetailsQueryResponse>> Handle(
     GetLoanDetailsQuery request,
     CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();

            const string loanSql = """
                SELECT 
                    l.Id,
                    e.Name AS EmployeeName,
                    l.Amount,
                    l.Months,
                    l.InstallmentAmount,
                    l.RemainingAmount,
                    l.StartDate,
                    l.Reason,
                    l.IsCompleted
                FROM HR.Loans l
                INNER JOIN HR.Employees e ON l.EmployeeId = e.Id
                WHERE l.Id = @LoanId
            """;

            const string installmentsSql = """
                SELECT 
                    i.Id,
                    ROW_NUMBER() OVER (ORDER BY i.DueDate) AS InstallmentNumber,
                    i.Amount,
                    i.DueDate,
                    i.IsPaid,
                    i.PaidAt
                FROM HR.LoanInstallments i
                WHERE i.LoanId = @LoanId
                ORDER BY i.DueDate
            """;

            var loan = await connection.QueryFirstOrDefaultAsync<dynamic>(
                loanSql, new { request.LoanId });

            if (loan == null)
                return Result<GetLoanDetailsQueryResponse>.Failure(LoanErrors.NotFoundLoan);

            var installments = await connection.QueryAsync<LoanInstallmentResponse>(
                installmentsSql, new { request.LoanId });

            var response = new GetLoanDetailsQueryResponse(
                Id: loan.Id,
                EmployeeName: loan.EmployeeName,
                StartDate: loan.StartDate,
                Amount: loan.Amount,
                Months: loan.Months,
                InstallmentAmount: loan.InstallmentAmount,
                RemainingAmount: loan.RemainingAmount,
                Reason: loan.Reason,
                IsCompleted: loan.IsCompleted,
                Installments: installments.ToList());

            return Result<GetLoanDetailsQueryResponse>.Success(response);
        }
    }
}
