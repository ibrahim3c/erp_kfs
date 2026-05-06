using HR.Domain.Payrolls;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class PayrollRepository : IPayrollRepository 
    {
        private readonly HRDbContext dbContext;

        public PayrollRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void AddPayrolAdjustment(PayrollAdjustment adjustment)
        {
            dbContext.PayrollAdjustments.Add(adjustment);
        }

        public void AddPayrollCycle(PayrollCycle payroll)
        {
            dbContext.PayrollCycles.Add(payroll);
        }

        public async Task<PayrollCycle?> GetCycleByMonthYearAsync(int month, int year, CancellationToken cancellationToken = default)
        {
            return await dbContext.PayrollCycles
           .FirstOrDefaultAsync(
            pc => pc.Month == month && pc.Year == year,
            cancellationToken);
        }

        public async Task<PayrollCycle> GetPayrollCycleByIdAsync(Guid cycleId, CancellationToken cancellationToken = default)
        {
            return await dbContext.PayrollCycles.FindAsync(cycleId, cancellationToken);
        }

        public async Task<PayrollEntry> GetPayrollEntryByIdAsync(Guid entryId, CancellationToken cancellationToken = default)
        {
            return await dbContext.PayrollEntries.FirstOrDefaultAsync(x => x.Id == entryId, cancellationToken);
        }
    }
}
