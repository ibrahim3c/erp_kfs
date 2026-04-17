using HR.Domain.Penalties;
using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.EditPenalty
{
    public record EditPenaltyCommand(
        Guid PenaltyId,
        DateTime ViolationDate,
        PenaltyActionType ActionType,
        string PenaltyType,
        decimal? DeductionDays,
        DateTime ExecutionMonth,
        string DecisionReference,
        string Notes,
        string? AttachmentPath
    ) : ICommand<Guid>;
}
