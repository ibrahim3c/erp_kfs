using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Loans
{
    public interface IInsurancePurchaseRepository
    {
        void Add(InsurancePeriodPurchase purchase);
        void Update(InsurancePeriodPurchase purchase);
        void Delete(InsurancePeriodPurchase purchase);
        Task<InsurancePeriodPurchase> GetByIdAsync(Guid id, CancellationToken cancellationToken);
         Task<List<InsurancePeriodPurchase>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken);
        Task<List<InsurancePeriodPurchase>> GetAllAsync(CancellationToken cancellationToken);

        // eagre load
        Task<List<InsurancePeriodPurchase>> GetApprovedByMonthAsync(int month, int year, CancellationToken cancellationToken = default);



    }
}
