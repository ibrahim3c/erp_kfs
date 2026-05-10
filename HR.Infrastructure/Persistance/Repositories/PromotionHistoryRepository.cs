using HR.Domain.Promotions.Entities;
using HR.Domain.Promotions.Interfaces;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class PromotionHistoryRepository : IPromotionHistoryRepository
    {
        private readonly HRDbContext _db;
        public PromotionHistoryRepository(HRDbContext db) => _db = db;

        public async Task AddRangeAsync(List<PromotionHistory> histories, CancellationToken ct)
        {
            await _db.PromotionHistories.AddRangeAsync(histories, ct);
 
        }

        public async Task<PromotionHistory?> GetLastByEmployeeAsync(
            Guid employeeId, CancellationToken ct)
            => await _db.PromotionHistories
                .Where(ph => ph.EmployeeId == employeeId)
                .OrderByDescending(ph => ph.EffectiveDate)
                .FirstOrDefaultAsync(ct);

        public async Task<List<PromotionHistory>> GetAllByEmployeeAsync(
            Guid employeeId, CancellationToken ct)
            => await _db.PromotionHistories
                .Where(ph => ph.EmployeeId == employeeId)
                .OrderByDescending(ph => ph.EffectiveDate)
                .Include(ph => ph.Cycle)
                .ToListAsync(ct);
    }
}
