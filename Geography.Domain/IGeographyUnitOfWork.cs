using CollegeControlSystem.Domain.Abstractions;

namespace Geography.Domain
{
    public interface IGeographyUnitOfWork : IUnitOfWork
    {
        IGeographyRepository GeographyRepository { get; }
    }
}
