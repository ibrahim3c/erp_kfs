using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Organization
{
    // Read-only repository because OrgUnitType is not an aggregate root,
    // and OrgUnit is the aggregate root that controls the business logic.
    public interface IOrgUnitTypeRepository
    {
        Task<OrgUnitType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<OrgUnitType>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
