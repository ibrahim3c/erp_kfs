using HR.Domain.Transfers.Enums;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Command.CreateExternalMovement
{
    public record CreateExternalMovementCommand(
     Guid EmployeeId, ExternalMovementType Type, MovementDirection Direction, string OtherEntityName,
     DateTime? StartDate, DateTime? EndDate, SalaryBearer? SalaryBearer,
     IFormFile? AttachmentFileName) : ICommand<Guid>;
}
