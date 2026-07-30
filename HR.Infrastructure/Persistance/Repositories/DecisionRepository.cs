using HR.Domain.Decisions;
using HR.Domain.Promotions.Enum;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class DecisionRepository : IDecisionRepository
    {
        private readonly HRDbContext _dbContext;

        private static readonly Dictionary<CycleType, string> _decisionCodes = new()
        {
            { CycleType.Promotion, "PROM_GRADE"   },
            { CycleType.Periodic,  "ALLOWANCE_7"  },
            { CycleType.Incentive, "ALLOWANCE_10" },
        };

        public DecisionRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Decision?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Decisions
                .Include(d => d.DecisionType)
                .Include(d => d.DecisionAuthority)
                .Include(d => d.EmployeeDecisions)
                .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<IReadOnlyList<Decision>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbContext.Decisions
                .Include(d => d.DecisionType)
                .Include(d => d.DecisionAuthority)
                .Include(d => d.EmployeeDecisions)
                .OrderByDescending(d => d.DecisionDate)
                .ToListAsync(ct);
        }

        public void Add(Decision decision)
        {
            _dbContext.Decisions.Add(decision);
        }

        public void AddEmployeeDecision(EmployeeDecision employeeDecision)
        {
            _dbContext.EmployeeDecisions.Add(employeeDecision);
        }

        public async Task<Guid> GetIdByMovementTypeAsync(CycleType type, CancellationToken ct)
        {
            if (!_decisionCodes.TryGetValue(type, out var code))
                return Guid.Empty;

            var decision = await _dbContext.DecisionTypes
                .Where(dt => dt.Code == code && dt.IsActive)
                .Select(dt => dt.Id)
                .FirstOrDefaultAsync(ct);

            return decision;
        }
    }
}
