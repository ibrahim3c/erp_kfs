using HR.Domain.Candidates;
using HR.Infrastructure.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance
{
    public class HRUnitOfWork // : IHRUnitOfWork
    {
        private readonly HRDbContext _dbContext;

        public ICandidateRepository Candidates { get; }

        public HRUnitOfWork(HRDbContext dbContext, ICandidateRepository candidateRepository)
        {
            _dbContext = dbContext;
            Candidates = candidateRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
