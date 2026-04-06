using HR.Domain.Candidates;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
namespace HR.Infrastructure.Persistance.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly HRDbContext _dbContext;

        public CandidateRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Candidate> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Candidates
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Candidate> GetByIdWithFilesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Include لجلب ملفات الترشيح مع المرشح
            return await _dbContext.Candidates
                .Include(c => c.NominationFiles)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Candidate> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Candidates
                .FirstOrDefaultAsync(c => c.NationalId == nationalId, cancellationToken);
        }

        public void Add(Candidate candidate)
        {
            _dbContext.Candidates.Add(candidate);
        }

        public void Update(Candidate candidate)
        {
            _dbContext.Candidates.Update(candidate);
        }

        public void Delete(Candidate candidate)
        {
            _dbContext.Candidates.Remove(candidate);
        }
    }
}
