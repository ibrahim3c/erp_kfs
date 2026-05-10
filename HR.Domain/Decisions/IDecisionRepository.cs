using HR.Domain.Promotions.Enum;

namespace HR.Domain.Decisions
{
    public interface IDecisionRepository
    {
        Task<Guid> GetIdByMovementTypeAsync(CycleType type, CancellationToken ct);
    }
}
