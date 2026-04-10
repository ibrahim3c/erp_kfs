

namespace Modules.Shared.Domain.Common.Governorates
{
    public interface IGovernorateRepository
    {
        Task<Governorate> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // (Eager Loading)
        Task<Governorate> GetByIdWithCityCentersAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<Governorate>> GetAllAsync(CancellationToken cancellationToken = default);

        void Add(Governorate candidate);
        void Update(Governorate candidate);
        void Delete(Governorate candidate);
    }
}
