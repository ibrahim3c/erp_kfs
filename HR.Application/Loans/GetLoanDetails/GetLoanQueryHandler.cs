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
        public async Task<Result<GetLoanDetailsQueryResponse>> Handle(GetLoanDetailsQuery request, CancellationToken cancellationToken)
        {
            // Using Dapper
            using var connection = connectionFactory.CreateConnection();
            var sql = """
                SELECT 
                    l."Id" AS LoanId,
                    e."Name" AS EmployeeName,
                    l."Amount" AS Amount,
                    l."Months" AS Months,
                    l."InstallmentAmount" AS InstallmentAmount,
                    l."RemainingAmount" AS RemainingAmount,
                    l."StartDate" AS StartDate,
                    l."Reason" AS Reason,
                    l."IsCompleted" AS IsCompleted,
                    l."CreatedAt" AS CreatedAt
                FROM 
                    "HR"."Loans" l
                INNER JOIN 
                    "HR"."Employees" e ON l."EmployeeId" = e."Id"
                WHERE 
                    l."Id" = @LoanId;
                """;

            var loan = await connection.QueryFirstOrDefaultAsync<GetLoanDetailsQueryResponse>(
                sql,
                new { LoanId = request.LoanId }
            );

            if (loan == null)
                return Result<GetLoanDetailsQueryResponse>.Failure(LoanErrors.NotFoundLoan);
            
            return Result<GetLoanDetailsQueryResponse>.Success(loan);
        }
    }
}
