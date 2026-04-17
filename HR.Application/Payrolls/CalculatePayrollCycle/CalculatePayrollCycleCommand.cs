using HR.Domain.Employees;
using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.CalculatePayrollCycle
{
    public record CalculatePayrollCycleCommand(
        int Month,
        int Year,
        Guid EmploymentTypeId
    ) : ICommand<Guid>;
}
