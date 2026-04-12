using Geography.Domain;
using Geography.Domain.IRepositories;
using Geography.Domain.Repositories;
using Geography.Infrastructure.Database;
using Geography.Infrastructure.Repositories;

namespace Geography.Infrastructure
{
    internal class GeographyUnitOfWork : IGeographyUnitOfWork
    {
        private readonly GeographyDbContext _dbContext;
        public GeographyUnitOfWork(GeographyDbContext dbContext)
        {
            _dbContext = dbContext;
            CityCenterRepository = new CityCenterRepository(_dbContext);
            LocalunitRepository = new LocalunitRepository(_dbContext);
            VillageRepository = new VillageRepository(_dbContext);
            GovernorateRepository = new GovernorateRepository(_dbContext);

        }
        public ICityCenterRepository CityCenterRepository { get; private set; }

        public ILocalunitRepository LocalunitRepository { get; private set; }

        public IVillageRepository VillageRepository { get; private set; }

        public IGovernorateRepository GovernorateRepository { get; private set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
