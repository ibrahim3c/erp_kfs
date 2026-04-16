using HR.Domain.Incentives;
using HR.Infrastructure.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class IncentiveRepository : IAcademicIncentiveRepository
    {
        private readonly HRDbContext dbContext;

        public IncentiveRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
