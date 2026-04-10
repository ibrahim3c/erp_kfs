using HR.Domain.Abstractions;
using HR.Domain.Candidates;
using HR.Domain.Employees;
using HR.Domain.Organization;
using HR.Infrastructure.Persistance.Database;
using HR.Infrastructure.Persistance.Repositories;
using Modules.Shared.Domain.Common.City_Center;
using Modules.Shared.Domain.Common.Governorates;
using Modules.Shared.Infrastructure.Presistance.Repositories;


namespace HR.Infrastructure.Persistance
{
    public class HRUnitOfWork  : IHRUnitOfWork
    {
        private readonly HRDbContext _dbContext;

        public ICandidateRepository Candidates { get; }

        public IEmployeeRepository Employees {  get; }

        public IOrgUnitRepository OrgUnits { get; }

        public IOrgUnitTypeRepository ReadOrgUnitTypes { get; }

        public IGovernorateRepository Governorate { get; }
        public ICityCenterRepository CityCenter { get; }


        public HRUnitOfWork(HRDbContext dbContext)
        {
            _dbContext = dbContext;
             Candidates = new CandidateRepository(_dbContext);
             Employees = new EmployeeRepository(_dbContext);
             OrgUnits = new OrgUnitRepository(_dbContext);
             ReadOrgUnitTypes = new OrgUnitTypeRepository(_dbContext);
             Governorate = new GovernorateRepository(_dbContext);
             CityCenter = new CityCenterRepository(_dbContext);
            
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
