using HR.Domain.Promotions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Interfaces
{
    public interface IPromotionHistoryRepository
    {
        Task AddRangeAsync(List<PromotionHistory> histories, CancellationToken ct);

        /// <summary>آخر حركة للموظف — لعرض التاريخ الوظيفي</summary>
        Task<PromotionHistory?> GetLastByEmployeeAsync(Guid employeeId, CancellationToken ct);

        /// <summary>كل حركات الموظف — Timeline كامل</summary>
        Task<List<PromotionHistory>> GetAllByEmployeeAsync(Guid employeeId, CancellationToken ct);
    }
}
