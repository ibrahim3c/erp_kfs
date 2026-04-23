using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Permissions
{
    public interface IPermissionRepository
    {
        Task<PermissionRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetMonthlyStatsDto> GetMonthlyStatsAsync(Guid employeeId, int month, int year, CancellationToken cancellationToken = default);
        void Add(PermissionRequest permission);
        void Update(PermissionRequest permission);
        void Delete(PermissionRequest permission);
    }
}
