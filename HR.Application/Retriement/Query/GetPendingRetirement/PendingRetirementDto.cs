using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Query.GetPendingRetirement
{
    public record PendingRetirementDto(
     Guid EmployeeId, string EmployeeName, string JobTitle,
     DateTime DateOfBirth, DateTime RetirementDate, Guid? RetirementFileId, string? FileStatus);
}
