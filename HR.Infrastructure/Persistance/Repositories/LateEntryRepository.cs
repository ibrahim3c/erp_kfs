using HR.Domain;
using HR.Domain.Permissions;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class LateEntryRepository : ILateEntryRepository
    {
        private readonly HRDbContext dbContext;

        public LateEntryRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public void Add(LateEntry entry)
        {
            dbContext.LateEntries.Add(entry);
        }

        public async Task<LateEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.LateEntries.FindAsync(id, cancellationToken);
        }

        public async Task<int> GetMonthlyLateMinutesAsync(Guid employeeId, int month, int year, CancellationToken cancellationToken = default)
        {
            return await dbContext.LateEntries
                .Where(e => e.EmployeeId == employeeId
                    && e.Date.Month == month
                    && e.Date.Year == year
                    && !e.IsTransferredToPenalty)
                .SumAsync(e => e.LateMinutes, cancellationToken);
        }

        public async Task<List<LateEntry>> GetPendingTransferAsync(Guid employeeId, int month, int year, CancellationToken cancellationToken = default)
        {
            return await dbContext.LateEntries
                .Where(e => e.EmployeeId == employeeId
                    && e.Date.Month == month
                    && e.Date.Year == year
                    && !e.IsTransferredToPenalty)
                .ToListAsync(cancellationToken);
        }
    }
}
