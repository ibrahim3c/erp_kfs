using CollegeControlSystem.Domain.Abstractions;


namespace Modules.Shared.Domain.Events
{
    public sealed record LeadershipPositionRemovedDomainEvent(DateTime RemovedAt,Guid EmployeeId) : IDomainEvent;

}
