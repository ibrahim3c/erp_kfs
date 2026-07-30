using Microsoft.AspNetCore.Http;
using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Command.CreateInternalTransfer
{
    public record CreateInternalTransferCommand(
      Guid EmployeeId, Guid FromDepartmentId, Guid ToDepartmentId, string Reason,
      DateTime ExecutionDate, Guid? NewJobTitleId,
      IFormFile? AttachmentFileName) : ICommand<Guid>;
}
