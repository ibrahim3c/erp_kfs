using Modules.Shared.Domain.Common.City_Center;
using Modules.Shared.Domain.Common.Governorates;

namespace CollegeControlSystem.Domain.Abstractions;

public interface IUnitOfWork
{
    IGovernorateRepository Governorate { get; }
    ICityCenterRepository CityCenter{ get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}