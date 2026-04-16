using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.CreateLoan
{
    public record CreateLoanCommand(
        Guid EmployeeId,
        decimal Amount,
        int Months,
        DateTime StartDate,
        string Reason) : ICommand<Guid>;
}
