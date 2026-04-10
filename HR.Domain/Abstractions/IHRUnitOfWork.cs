using CollegeControlSystem.Domain.Abstractions;
using HR.Domain.Candidates;
using HR.Domain.Employees;
using HR.Domain.Organization;


namespace HR.Domain.Abstractions
{
    public interface IHRUnitOfWork : IUnitOfWork
    {
        ICandidateRepository Candidates { get; }
        IEmployeeRepository Employees { get; }
        IOrgUnitRepository OrgUnits { get; }
        IOrgUnitTypeRepository ReadOrgUnitTypes { get; }
    }
}
