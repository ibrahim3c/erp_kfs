using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Query.List
{
    public record TerminationListItemDto(
      Guid Id, string DecisionNumber, string EmployeeName, string Reason,
      DateTime DecisionDate, DateTime LastWorkingDay, string? AttachmentPath, string Status);

}
