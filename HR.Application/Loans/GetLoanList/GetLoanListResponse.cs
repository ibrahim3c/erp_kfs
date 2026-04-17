using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.GetLoanList
{
  
    public sealed record GetLoanListResponse(
        Guid Id,
        string EmployeeName,
        DateTime StartDate,
        decimal Amount,
        int Months,
        decimal InstallmentAmount,
        decimal RemainingAmount,
        bool IsCompleted);

   
}
