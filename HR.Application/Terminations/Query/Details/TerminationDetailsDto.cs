using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Query.Details
{
    public record TerminationDetailsDto(
        Guid Id,
        string DecisionNumber,
        string EmployeeName,
        string JobTitle,
        string Reason,
        DateTime DecisionDate,
        DateTime LastWorkingDay,
        string? LegalBasis,
        string? AttachmentPath,
        string Status,
        string? CancellationReason,
        DateTime? UpdatedOn
    );
}
