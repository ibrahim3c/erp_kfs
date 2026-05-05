using CollegeControlSystem.Domain.Abstractions;


namespace HR.Domain.Employees.Events
{
    public sealed record EmployeeHiredDomainEvent(
     Guid EmployeeId,
     DateTime HiredAt) : IDomainEvent;
}
