using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.CreateInsurancePurchase
{
    public record CreateInsurancePurchaseCommand(
         Guid EmployeeId,
         string InsuranceAuthority,
         int PurchasedYears,
         decimal TotalCost,
         decimal MonthlyInstallment,
         DateTime DeductionStartDate,
         string? ApprovalDecisionFilePath
     ) : ICommand<Guid>;
}
