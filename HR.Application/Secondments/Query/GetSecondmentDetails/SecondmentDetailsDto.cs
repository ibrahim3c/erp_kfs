using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Query.GetSecondmentDetails
{

    public record SecondmentDetailsDto(
        Guid Id, Guid EmployeeId, string EmployeeName, string JobTitle,
        string Type, string HostEntityName, DateTime StartDate, DateTime EndDate,
        string SalaryBearer, string IncentiveBearer, bool ClearanceCompleted,
        string Status, string? FilePath);
}
