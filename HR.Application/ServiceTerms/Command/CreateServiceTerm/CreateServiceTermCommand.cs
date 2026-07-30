using HR.Domain.ServiceTerms.Enums;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Command.CreateServiceTerm
{
    public record CreateServiceTermCommand(
    Guid EmployeeId, string PreviousEntityName, ServiceType Type,
    DateTime StartDate, DateTime EndDate, string? CommitteeDecisionNumber,
    IFormFile? AttachmentFileName) : ICommand<Guid>;
 
}
