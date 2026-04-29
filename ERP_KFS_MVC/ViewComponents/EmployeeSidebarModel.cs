namespace ERP_KFS_MVC.ViewComponents
{
    public class EmployeeSidebarModel
    {
        public string FullName { get; set; } = "موظف";
        public bool IsManager { get; set; } = false;
        public List<string> PermissionsList { get; set; } = new();
        public List<string> UserRoles { get; set; } = new();

        public bool IsAdmin => UserRoles.Contains("Admin");
        public bool IsHR => UserRoles.Contains("HR");
        public bool IsEmployee => !IsAdmin && !IsHR;

        public bool HasPermission(string permission) => PermissionsList.Contains(permission);
    }
}