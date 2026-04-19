using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using MyERP.Web.Models;
using erp_kfs.Host.Models;

namespace MyERP.Web.ViewComponents
{
    public class EmployeeSidebarViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmployeeSidebarViewComponent(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return View(new EmployeeSidebarModel { FullName = "موظف" });

            var employee = await _context.EmployeeAdmins
                .FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);

            if (employee == null)
                return View(new EmployeeSidebarModel { FullName = user.FullName ?? "موظف" });

            // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
            // ✅ التحقق من كونه مدير (باستخدام أي دي الموظف)
            var isManager = await _context.Departments
                .AnyAsync(d => d.ManagerId == employee.Id);
            // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←

            // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
            // ✅ جلب الأدوار (التحويل الصريح باستخدام ToList)
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←

            // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
            // ✅ جلب الصلاحيات من الأدوار (مع تحميل العلاقة Role)
            var rolePermissions = await _context.RolePermissions
                .Include(rp => rp.Role) // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
                .Where(rp => rp.Role != null && userRoles.Contains(rp.Role.Name)) // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
                .Select(rp => rp.Permission)
                .ToListAsync();
            // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←

            // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
            // ✅ جلب الصلاحيات المباشرة للمستخدم
            var directPermissions = await _context.UserPermissions
                .Where(up => up.UserId == user.Id)
                .Select(up => up.Permission)
                .ToListAsync();
            // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←

            var allPermissions = rolePermissions.Union(directPermissions).ToList();

            var model = new EmployeeSidebarModel
            {
                FullName = user.FullName ?? "موظف",
                IsManager = isManager,
                Permissions = allPermissions,
                UserRoles = userRoles
            };

            return View(model);
        }
    }
}