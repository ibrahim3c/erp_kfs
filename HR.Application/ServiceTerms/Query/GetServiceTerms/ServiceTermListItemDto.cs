using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Query.GetServiceTerms
{
    public record ServiceTermListItemDto(
     Guid Id, Guid EmployeeId, string EmployeeName, string PreviousEntityName,
     DateTime StartDate, DateTime EndDate, string Status,
     DateTime? AdjustedSeniorityDate, string? AttachmentPath);
}
