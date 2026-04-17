using HR.Domain.Terminations;
using HR.Infrastructure.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class TerminationRepository : ITerminationRepository
    {
        private readonly HRDbContext dbContext;

        public TerminationRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
