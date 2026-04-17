using HR.Domain.Decisions;
using HR.Infrastructure.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class DecisionRepository : IDecisionRepository
    {
        private readonly HRDbContext dbContext;

        public DecisionRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
