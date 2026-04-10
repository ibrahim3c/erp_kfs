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
    public class OrgUnitRepository : IOrgUnitRepository
    {
        private readonly HRDbContext dbContext;

        public OrgUnitRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public void Add(OrgUnit orgUnit)
        {
            dbContext.OrgUnits.Add(orgUnit);
        }

        public async Task AddAsync(OrgUnit orgUnit, CancellationToken cancellationToken = default)
        {
            await dbContext.OrgUnits.AddAsync(orgUnit, cancellationToken);
        }

        public void Delete(OrgUnit orgUnit)
        {
            dbContext.OrgUnits.Remove(orgUnit);
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await dbContext.OrgUnits.AnyAsync(o => o.Code == code);
            
        }

        public async Task<OrgUnit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.OrgUnits.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public void Update(OrgUnit orgUnit)
        {
            dbContext.OrgUnits.Update(orgUnit);
        }
    }
}
