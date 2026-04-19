using erp_kfs.Host.Models;

namespace MyERP.Web.Models
{
    public class EmployeeProfileViewModel
    {
        public EmployeeAdmin Employee { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public List<Permission> Permissions { get; set; } = new();
        public List<LeaveRequest> LeaveRequests { get; set; } = new();
        public List<ServicePeriodAddition> ServicePeriods { get; set; } = new();
        public List<TerminationRequest> TerminationRequests { get; set; } = new();
    }
}