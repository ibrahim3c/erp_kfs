using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Penalties
{
    public interface IPenaltyRepository
    {
        void Add(PenaltyRecord penalty);
        Task<PenaltyRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<PenaltyRecord>> GetAllAsync(CancellationToken cancellationToken = default);
         Task<List<PenaltyRecord>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
        Task<decimal?> GetTotalDaysAsync(Guid employeeId, DateTime fromDate, CancellationToken ct);
        void Update(PenaltyRecord penalty);
         void Delete(PenaltyRecord penalty);
    }

}
