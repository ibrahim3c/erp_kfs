using CollegeControlSystem.Domain.Abstractions;

namespace Organization.Domain
{
    public interface IOrganizationUnitOfWork:IUnitOfWork
    {
            IOrganizationRepository OrganizationRepository { get; }
        }
}
