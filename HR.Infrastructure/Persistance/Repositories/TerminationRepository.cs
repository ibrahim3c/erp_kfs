using HR.Domain.Terminations;
using HR.Domain.Terminations.Enums;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
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

        public void Add(TerminationDecision terminationDecision)
        {
            dbContext.TerminationDecisions.Add(terminationDecision);
        }

        public async Task<bool> AnyAsync(Guid employeeId, TerminationStatus status, CancellationToken cancellationToken = default)
        {
            return await dbContext.TerminationDecisions.AnyAsync(td => td.EmployeeId == employeeId && td.Status == status, cancellationToken);
        }

        public void Delete(TerminationDecision terminationDecision)
        {
            dbContext.TerminationDecisions.Remove(terminationDecision);
        }

        public async Task<TerminationDecision?> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            return await dbContext.TerminationDecisions.FirstOrDefaultAsync(td => td.EmployeeId == employeeId, cancellationToken);
        }

        public async Task<TerminationDecision?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.TerminationDecisions.FirstOrDefaultAsync(td => td.Id == id, cancellationToken);
        }
    }
}
