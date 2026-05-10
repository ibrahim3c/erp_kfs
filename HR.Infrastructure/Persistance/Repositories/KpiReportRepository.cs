using HR.Domain.Promotions.Entities;
using HR.Domain.Promotions.Interfaces;
using HR.Domain.Promotions.Services;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    internal class KpiReportRepository : IKpiReportRepository
    {
        private readonly HRDbContext _db;
        public KpiReportRepository(HRDbContext db) => _db = db;

        public void Add(KpiReport kpiReport)
        {
            _db.KpiReports.Add(kpiReport);
        }

        public async Task<List<KpiReportDto>> GetByEmployeeAsync(Guid employeeId, int yearsBack, CancellationToken ct)
        {
            var fromYear = DateTime.Now.Year - yearsBack;

            return await _db.KpiReports
                .Where(k => k.EmployeeId == employeeId && k.Year > fromYear)
                .OrderByDescending(k => k.Year)
                .Select(k => new KpiReportDto(k.Year, k.Score))
                .ToListAsync(ct);
        }



    }
}
