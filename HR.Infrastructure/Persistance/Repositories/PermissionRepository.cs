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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly HRDbContext dbContext;

        public PermissionRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void Add(PermissionRequest permission)
        {
            dbContext.PermissionRequests.Add(permission);
        }

        public void Delete(PermissionRequest permission)
        {
            dbContext.PermissionRequests.Remove(permission);
        }

        public async Task<PermissionRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.PermissionRequests.FindAsync(id, cancellationToken);
        }

        public async Task<GetMonthlyStatsDto> GetMonthlyStatsAsync(Guid employeeId, int month, int year, CancellationToken cancellationToken = default)
        {
            var stats = await dbContext.PermissionRequests
                .Where(p => p.EmployeeId == employeeId
                    && p.Date.Month == month
                    && p.Date.Year == year)
                .GroupBy(p => 1)
                .Select(g => new
                {
                    Count = g.Count(),
                    TotalMinutes = g.Sum(p => p.DurationMinutes) // تم التعديل هنا
                })
                .FirstOrDefaultAsync(cancellationToken);

            return new GetMonthlyStatsDto
            {
                Count = stats?.Count ?? 0,
                TotalMinutes = stats?.TotalMinutes ?? 0
            };
        }

        public void Update(PermissionRequest permission)
        {
           dbContext.PermissionRequests.Update(permission);
        }
    }
}
