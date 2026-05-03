using CollegeControlSystem.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Shared.Domain.Events
{
    public sealed record LeadershipPositionAssignedDomainEvent(
       Guid EmployeeId,
       Guid LeadershipPositionId,
       DateTime AssignedAt,
       string? Notes) : IDomainEvent;
}
