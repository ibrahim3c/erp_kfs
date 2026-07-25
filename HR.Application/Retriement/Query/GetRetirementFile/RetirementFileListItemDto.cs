using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Query.GetRetirementFile
{
    public record RetirementFileListItemDto(
    Guid Id, Guid EmployeeId, string EmployeeName, DateTime ReferralDate,
    string Reason, string Stage, DateTime UpdatedOn, string? ResponsibleName);
}
