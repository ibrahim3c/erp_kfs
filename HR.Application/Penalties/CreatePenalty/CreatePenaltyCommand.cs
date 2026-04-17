using HR.Domain.Penalties;
using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.CreatePenalty
{
    public record CreatePenaltyCommand(
        Guid EmployeeId,           
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
