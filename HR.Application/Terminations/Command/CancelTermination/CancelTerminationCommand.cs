using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Command.CancelTermination
{
    public record CancelTerminationCommand(Guid TerminationId, string CancellationReason) : ICommand;
}
