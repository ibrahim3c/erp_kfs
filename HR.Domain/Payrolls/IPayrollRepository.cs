using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Payrolls
{
    public interface IPayrollRepository
    {
        Task<PayrollEntry> GetPayrollEntryByIdAsync(Guid entryId, CancellationToken cancellationToken = default);
        Task<PayrollCycle> GetPayrollCycleByIdAsync(Guid cycleId, CancellationToken cancellationToken = default);
        void AddPayrollCycle(PayrollCycle payroll);
    }
}
