using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Query.GetServiceTermDetails
{

    public record ServiceTermDetailsDto(
        Guid Id, string EmployeeName, string PreviousEntityName, string Type,
        DateTime StartDate, DateTime EndDate, string Status,
        DateTime? AdjustedSeniorityDate, string? RejectionReason,
        string? CommitteeDecisionNumber, string? AttachmentPath);
}
