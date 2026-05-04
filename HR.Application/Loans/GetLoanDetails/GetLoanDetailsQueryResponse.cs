using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.GetLoanDetails
{
    public sealed record GetLoanDetailsQueryResponse(
       Guid Id,
       string EmployeeName,
       DateTime StartDate,
       decimal Amount,
       int Months,
       decimal InstallmentAmount,
       decimal RemainingAmount,
       string reason,
       bool IsCompleted);
}
