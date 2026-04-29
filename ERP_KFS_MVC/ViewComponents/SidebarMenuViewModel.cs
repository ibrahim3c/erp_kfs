namespace ERP_KFS_MVC.ViewComponents
{
    public class SidebarMenuViewModel
    {
        public bool IsManager { get; set; }
        public int PendingLeaveRequestsCount { get; set; }
        public List<string> UserRoles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();

        public bool IsAdmin => UserRoles.Contains("Admin");
        public bool IsHR => UserRoles.Contains("HR");
        public bool HasPermission(string permission) => Permissions.Contains(permission);
    }
}