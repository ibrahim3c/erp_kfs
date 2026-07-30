using Modules.Shared.Domain;

namespace HR.Domain.Evaluations
{
    public interface IGrievanceRepository
    {
        Task<Grievance?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Grievance>> GetAllAsync(CancellationToken ct = default);
        void Add(Grievance grievance);
        void Update(Grievance grievance);
    }
}
