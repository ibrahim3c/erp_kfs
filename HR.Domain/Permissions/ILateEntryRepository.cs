using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Permissions
{
    public interface ILateEntryRepository
    {
        Task<LateEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Add(LateEntry entry);

        Task<int> GetMonthlyLateMinutesAsync(
            Guid employeeId, int month, int year, CancellationToken cancellationToken = default);

        Task<List<LateEntry>> GetPendingTransferAsync(
            Guid employeeId, int month, int year, CancellationToken cancellationToken = default);
    }
}
