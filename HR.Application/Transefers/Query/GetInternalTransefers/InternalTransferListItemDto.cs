using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Query.GetInternalTransefers
{
    public record InternalTransferListItemDto(
      Guid Id, Guid EmployeeId, string EmployeeName, string FromDepartmentName, string ToDepartmentName, string NewJobTitleName,
      string Reason, DateTime ExecutionDate, string Status);
}
