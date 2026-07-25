using HR.Domain.Retirement.Entities;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class RetriementRepository : IRetriementRepository
    {
        private readonly HRDbContext _dbContext;

        public RetriementRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(RetirementFile retirement)
        {
            _dbContext.RetirementFiles.Add(retirement);
        }

        public async Task<RetirementFile?> GetByIdAsync(Guid retirementId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RetirementFiles.Include(r => r.SalaryRecords)
                .FirstOrDefaultAsync(r => r.Id == retirementId, cancellationToken);
        }
    }
}
