using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Organization
{
    public interface IOrgUnitRepository
    {
        Task<OrgUnit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(OrgUnit orgUnit, CancellationToken cancellationToken = default);
        void Add(OrgUnit orgUnit);
        void Update(OrgUnit orgUnit);
        void Delete(OrgUnit orgUnit);

        Task<bool> ExistsByCodeAsync(string code);
    }
}
