using HR.Domain.Evaluations;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace HR.Infrastructure.Persistance.Repositories
{
    internal class GrievanceRepository : IGrievanceRepository
    {
        private readonly HRDbContext _dbContext;

        public GrievanceRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Grievance?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Grievances
                .Include(g => g.Employee)
                .FirstOrDefaultAsync(g => g.Id == id, ct);
        }

        public async Task<IReadOnlyList<Grievance>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbContext.Grievances
                .Include(g => g.Employee)
                .OrderByDescending(g => g.SubmissionDate)
                .ToListAsync(ct);
        }

        public void Add(Grievance grievance)
        {
            _dbContext.Grievances.Add(grievance);
        }

        public void Update(Grievance grievance)
        {
            _dbContext.Grievances.Update(grievance);
        }
    }
}
