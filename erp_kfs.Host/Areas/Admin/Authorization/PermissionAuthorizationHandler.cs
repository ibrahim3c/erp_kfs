using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using erp_kfs.Host.Models;
using System.Security.Claims;
using MyERP.Web.Data;

namespace erp_kfs.Host.Areas.Admin.Authorization
{
    // ══════════════════════════════════════════════════════
    // 1. تعريف الـ Requirement
    // ══════════════════════════════════════════════════════
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
    }

    // ══════════════════════════════════════════════════════
    // 2. الـ Handler اللي بيتحقق من الصلاحية (مُصلح بدون أخطاء)
    // ══════════════════════════════════════════════════════
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
        // ✅ التسجيل الصحيح للخدمات (بدون استخدام ServiceScope في الـ Handler)
        public PermissionAuthorizationHandler(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (!context.User.Identity?.IsAuthenticated ?? false)
                return;

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            // ✅ Admin عنده كل الصلاحيات تلقائياً
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            // ✅ التحقق من الصلاحيات المباشرة
            var hasDirectPermission = await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.Permission.Name == requirement.PermissionName);

            if (hasDirectPermission)
            {
                context.Succeed(requirement);
                return;
            }

            // ✅ التحقق من صلاحيات الأدوار (بدون خطأ في الـ null)
            var userRoles = await _userManager.GetRolesAsync(user);
            var hasRolePermission = await _context.RolePermissions
                .Where(rp => userRoles.Contains(rp.Role.Name) && rp.Permission.Name == requirement.PermissionName)
                .AnyAsync();

            if (hasRolePermission)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }

    // ══════════════════════════════════════════════════════
    // 3. Extension Method لتسجيل الـ Policies (مُحدّث)
    // ══════════════════════════════════════════════════════
    public static class PermissionPolicyProvider
    {
        public static void AddPermissionPolicies(this AuthorizationOptions options)
        {
            // لوحة التحكم
            options.AddPolicy("Permission.Dashboard.View",
                policy => policy.Requirements.Add(new PermissionRequirement("Dashboard.View")));

            // الموظفين
            options.AddPolicy("Permission.Employees.View",
                policy => policy.Requirements.Add(new PermissionRequirement("Employees.View")));
            options.AddPolicy("Permission.Employees.Create",
                policy => policy.Requirements.Add(new PermissionRequirement("Employees.Create")));

            // الإدارات
            options.AddPolicy("Permission.Departments.Manage",
                policy => policy.Requirements.Add(new PermissionRequirement("Departments.Manage")));

            // القيادات
            options.AddPolicy("Permission.Leadership.Manage",
                policy => policy.Requirements.Add(new PermissionRequirement("Leadership.Manage")));

            // الإجازات
            options.AddPolicy("Permission.Leaves.Regular",
                policy => policy.Requirements.Add(new PermissionRequirement("Leaves.Regular")));
            options.AddPolicy("Permission.Leaves.ManagerApprove",
                policy => policy.Requirements.Add(new PermissionRequirement("Leaves.ManagerApprove")));

            // ضم المدة
            options.AddPolicy("Permission.ServiceTerm.Manage",
                policy => policy.Requirements.Add(new PermissionRequirement("ServiceTerm.Manage")));

            // إنهاء الخدمة
            options.AddPolicy("Permission.Terminations.View",
                policy => policy.Requirements.Add(new PermissionRequirement("Terminations.View")));
            options.AddPolicy("Permission.Terminations.Manage",
                policy => policy.Requirements.Add(new PermissionRequirement("Terminations.Manage")));

            // المرتبات
            options.AddPolicy("Permission.Payroll.Generate",
                policy => policy.Requirements.Add(new PermissionRequirement("Payroll.Generate")));

            // الصلاحيات (Admin فقط)
            options.AddPolicy("Permission.Permissions.View",
                policy => policy.Requirements.Add(new PermissionRequirement("Permissions.View")));
        }
    }
}