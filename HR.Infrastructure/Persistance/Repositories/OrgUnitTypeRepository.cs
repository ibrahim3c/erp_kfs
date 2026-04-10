using HR.Domain.Organization;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    // Read-only repository because OrgUnitType is not an aggregate root,
   // and OrgUnit is the aggregate root that controls the business logic.
    public class OrgUnitTypeRepository : IOrgUnitTypeRepository
    {
        private readonly HRDbContext dbContext;

        public OrgUnitTypeRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<OrgUnitType>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.OrgUnitTypes.ToListAsync(cancellationToken);
        }

        public async Task<OrgUnitType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.OrgUnitTypes.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }
    }
}
