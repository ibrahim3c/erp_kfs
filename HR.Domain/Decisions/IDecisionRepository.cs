using HR.Domain.Promotions.Enum;

namespace HR.Domain.Decisions
{
    public interface IDecisionRepository
    {
        Task<Decision?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Decision>> GetAllAsync(CancellationToken ct = default);
        void Add(Decision decision);
        void AddEmployeeDecision(EmployeeDecision employeeDecision);
        Task<Guid> GetIdByMovementTypeAsync(CycleType type, CancellationToken ct);
    }
}
