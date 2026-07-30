using HR.Application.Employees.GetAllEmployees;
using HR.Application.Leaves.GetLeaveBalance;
using HR.Application.Leaves.GetMedicalLeaveRequests;
using HR.Application.Leaves.GetRegularLeaveRequests;
using HR.Application.Leaves.GetSpecialLeaveRequests;

namespace ERP_KFS_MVC.Areas.HR.ViewModels
{
    public class LeavePageViewModel
    {
        public List<GetRegularLeaveRequestsResponse> RegularRequests { get; set; } = new();
        public List<GetSpecialLeaveRequestsResponse> SpecialRequests { get; set; } = new();
        public List<GetMedicalLeaveRequestsResponse> MedicalRequests { get; set; } = new();
        public GetLeaveBalanceResponse? Balance { get; set; }
        public IEnumerable<EmployeeListResponse> Employees { get; set; } = Enumerable.Empty<EmployeeListResponse>();
    }
}
