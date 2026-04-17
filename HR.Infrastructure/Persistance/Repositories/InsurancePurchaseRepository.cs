using HR.Domain.Loans;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class InsurancePurchaseRepository : IInsurancePurchaseRepository
    {
        private readonly HRDbContext dbContext;

        public InsurancePurchaseRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public void Add(InsurancePeriodPurchase purchase)
        {
            dbContext.InsurancePeriodPurchases.Add(purchase);
        }

        public void Delete(InsurancePeriodPurchase purchase)
        {
            throw new NotImplementedException();
        }

        public async Task<List<InsurancePeriodPurchase>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbContext.InsurancePeriodPurchases.Include(i => i.Employee).ToListAsync(cancellationToken);
        }

        public async Task<List<InsurancePeriodPurchase>> GetApprovedByMonthAsync(int month, int year, CancellationToken cancellationToken = default)
        {
            return await dbContext.InsurancePeriodPurchases
                         .Where(ip => ip.Status == InsurancePurchaseStatus.Approved
                                   && ip.DeductionStartDate <= new DateTime(year, month, 1))
                         .ToListAsync(cancellationToken);
        }

        public Task<List<InsurancePeriodPurchase>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<InsurancePeriodPurchase> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
           return await dbContext.InsurancePeriodPurchases
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public void Update(InsurancePeriodPurchase purchase)
        {
            throw new NotImplementedException();
        }
    }
}
