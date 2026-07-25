using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Query.GetRetirementFileDetails
{
    public record RetirementFileDetailsDto(
     Guid Id, Guid EmployeeId, string EmployeeName, DateTime ReferralDate,
     bool JoinPeriodsAdded, bool SpecialLeavesReviewed,
     string? Notes, List<SalaryYearDto> SalaryRecords);
}
