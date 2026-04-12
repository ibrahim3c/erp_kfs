using CollegeControlSystem.Domain.Abstractions;
using Geography.Domain.IRepositories;
using Geography.Domain.Repositories;

namespace Geography.Domain
{
    public interface IGeographyUnitOfWork : IUnitOfWork
    {
        ICityCenterRepository CityCenterRepository { get; }
        ILocalunitRepository LocalunitRepository { get; }
        IVillageRepository VillageRepository { get; }
        IGovernorateRepository GovernorateRepository { get; }

    }
}
