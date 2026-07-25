using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Query.GetActiveSecondments
{
    public record SecondmentListItemDto(
     Guid Id, Guid EmployeeId, string EmployeeName, string Type,
     string HostEntityName, DateTime StartDate, DateTime EndDate,
     string SalaryBearer, string Status, bool ClearanceCompleted);
}
