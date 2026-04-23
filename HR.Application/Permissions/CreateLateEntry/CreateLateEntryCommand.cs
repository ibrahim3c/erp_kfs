using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Permissions.CreateLateEntry
{
    public record CreateLateEntryCommand(
       Guid EmployeeId,
       DateTime Date,
       TimeSpan ActualArrivalTime,
       string? Notes
   ) : ICommand<Guid>;
}
