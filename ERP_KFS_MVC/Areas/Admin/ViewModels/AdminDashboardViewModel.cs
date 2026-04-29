namespace ERP_KFS_MVC.Areas.Admin.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalEmployees { get; set; }
        public int PendingLeaveRequests { get; set; }
        public int PendingHRRequests { get; set; }
        public int DepartmentsWithoutManagers { get; set; }
    }
}