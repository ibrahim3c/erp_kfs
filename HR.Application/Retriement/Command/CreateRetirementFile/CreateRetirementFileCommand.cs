using HR.Domain.Retirement.Enums;
using MediatR;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Command.CreateRetirementFile
{
    public record CreateRetirementFileCommand(Guid EmployeeId, DateTime ReferralDate, RetirementReason Reason, Guid? ResponsibleEmployeeId)
      : ICommand<Guid >;
}
 