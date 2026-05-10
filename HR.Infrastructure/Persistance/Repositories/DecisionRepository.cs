using HR.Domain.Decisions;
using HR.Domain.Promotions.Enum;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class DecisionRepository : IDecisionRepository
    {
        private readonly HRDbContext dbContext;
        private static readonly Dictionary<CycleType, string> _decisionCodes = new()
        {
            { CycleType.Promotion, "PROM_GRADE"   },  // ترقية درجة
            { CycleType.Periodic,  "ALLOWANCE_7"  },  // علاوة دورية 7%
            { CycleType.Incentive, "ALLOWANCE_10" },  // علاوة تشجيعية 10%
        };


        public DecisionRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<Guid> GetIdByMovementTypeAsync(CycleType type, CancellationToken ct)
        {
            if (!_decisionCodes.TryGetValue(type, out var code))
                return Guid.Empty;

            // عدّل اسم الجدول / العمود حسب جدول الـ Settings بتاعك
            var decision = await dbContext.DecisionTypes
                .Where(dt => dt.Code == code && dt.IsActive)
                .Select(dt => dt.Id)
                .FirstOrDefaultAsync(ct);

            return decision;
        }

        public void AddEmployeeDecision(EmployeeDecision employeeDecision)
        {
             dbContext.EmployeeDecisions.Add(employeeDecision);
           
        }
    }
}
