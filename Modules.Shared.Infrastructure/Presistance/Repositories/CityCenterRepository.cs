using Microsoft.EntityFrameworkCore;
using Modules.Shared.Domain.Common.City_Center;
using Modules.Shared.Infrastructure.Presistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Shared.Infrastructure.Presistance.Repositories
{
    public class CityCenterRepository : ICityCenterRepository
    {
        private readonly SharedDbContext dbContext;

        public CityCenterRepository(SharedDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public void Add(CityCenter candidate)
        {
            dbContext.CityCenters.Add(candidate);
        }

        public void Delete(CityCenter candidate)
        {
            dbContext.CityCenters.Remove(candidate);
        }

        public async Task<List<CityCenter>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.CityCenters.ToListAsync(cancellationToken);
        }
      
        public async Task<CityCenter> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.CityCenters.FindAsync(id, cancellationToken);
        }

        public void Update(CityCenter candidate)
        {
            dbContext.CityCenters.Update(candidate);
        }
    }
}
