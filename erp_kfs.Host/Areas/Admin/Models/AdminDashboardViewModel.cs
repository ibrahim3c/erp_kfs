// في Models/AdminDashboardViewModel.cs
namespace MyERP.Web.Areas.Admin.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int PendingLeaveRequests { get; set; }
        public int PendingHRRequests { get; set; }
        public int DepartmentsWithoutManagers { get; set; }
    }
}