using HR.Domain.Promotions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Interfaces
{
    public interface IPromotionCycleRepository
    {

        Task<Guid> SaveCycleAsync(PromotionCycle cycle, CancellationToken ct);
        void Update(PromotionCycle cycle);
        Task<PromotionCycle?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddResultsAsync(IEnumerable<EligibilityResult> results, CancellationToken ct);

    }
}
