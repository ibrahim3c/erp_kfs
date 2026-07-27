using HR.Domain.Terminations.Enums;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Command.CreateTermination
{
    public record CreateTerminationCommand(
    Guid EmployeeId, string DecisionNumber, TerminationReason Reason,
    DateTime DecisionDate, DateTime LastWorkingDay, string? LegalBasis, IFormFile? AttachmentFile) : ICommand<Guid>;
}
