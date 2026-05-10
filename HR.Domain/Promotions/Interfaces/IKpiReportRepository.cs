using HR.Domain.Promotions.Entities;
using HR.Domain.Promotions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Interfaces
{
    public interface IKpiReportRepository
    {
        Task<List<KpiReportDto>> GetByEmployeeAsync(Guid employeeId, int yearsBack, CancellationToken ct);
         void Add(KpiReport kpiReport);

    }
}
