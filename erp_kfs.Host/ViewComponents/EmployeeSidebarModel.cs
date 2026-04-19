namespace MyERP.Web.ViewComponents
{
    public class EmployeeSidebarModel
    {
        public string FullName { get; set; } = "موظف";
        public bool IsManager { get; set; } = false;
        public List<MyERP.Web.Models.Permission> Permissions { get; set; } = new();
        public List<string> UserRoles { get; set; } = new();

        // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
        // ✅ خصائص محسوبة للتمييز بين الأدوار (بدون حاجة لتعديل الكونترولر)
        public bool IsAdmin => UserRoles.Contains("Admin");
        public bool IsHR => UserRoles.Contains("HR");
        public bool IsEmployee => !IsAdmin && !IsHR;
        // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
    }
}