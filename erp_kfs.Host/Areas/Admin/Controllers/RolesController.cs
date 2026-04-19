using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using MyERP.Web.Areas.Admin.Models;
using MyERP.Web.Models;
using erp_kfs.Host.Models;

namespace MyERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        // ══════════════════════════════════════════════════════
        // عرض كل الأدوار
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();

            // حساب عدد المستخدمين والصلاحيات لكل دور
            var roleStats = new Dictionary<string, RoleStatsViewModel>();
            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                var permCount = await _context.RolePermissions
                    .CountAsync(rp => rp.RoleId == role.Id);

                roleStats[role.Id] = new RoleStatsViewModel
                {
                    UserCount = usersInRole.Count,
                    PermissionCount = permCount
                };
            }

            ViewBag.RoleStats = roleStats;
            return View(roles);
        }

        // ══════════════════════════════════════════════════════
        // إنشاء دور جديد
        // ══════════════════════════════════════════════════════
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // التحقق من عدم التكرار
            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "هذا الدور موجود بالفعل.");
                return View(model);
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
            if (result.Succeeded)
            {
                TempData["Success"] = $"تم إنشاء الدور '{model.Name}' بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // ══════════════════════════════════════════════════════
        // تعديل اسم الدور
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            // منع تعديل الأدوار الأساسية
            var protectedRoles = new[] { "Admin", "HR", "Recruiter", "Employee" };
            ViewBag.IsProtected = protectedRoles.Contains(role.Name);

            return View(new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name ?? "",
                Description = "" // ممكن تضيف حقل Description للـ Role لاحقاً
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null) return NotFound();

            // منع تعديل الأدوار الأساسية
            var protectedRoles = new[] { "Admin", "HR", "Recruiter", "Employee" };
            if (protectedRoles.Contains(role.Name))
            {
                TempData["Error"] = "لا يمكن تعديل الأدوار الأساسية للنظام.";
                return RedirectToAction(nameof(Index));
            }

            // التحقق من عدم تكرار الاسم الجديد
            if (role.Name != model.Name && await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "هذا الاسم مستخدم بالفعل.");
                return View(model);
            }

            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                TempData["Success"] = $"تم تعديل الدور بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // ══════════════════════════════════════════════════════
        // حذف دور
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            // منع حذف الأدوار الأساسية
            var protectedRoles = new[] { "Admin", "HR", "Recruiter", "Employee" };
            if (protectedRoles.Contains(role.Name))
            {
                TempData["Error"] = "لا يمكن حذف الأدوار الأساسية للنظام.";
                return RedirectToAction(nameof(Index));
            }

            // التحقق من وجود مستخدمين
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                TempData["Error"] = $"لا يمكن حذف الدور '{role.Name}' لأنه مُعيّن لـ {usersInRole.Count} مستخدم.";
                return RedirectToAction(nameof(Index));
            }

            // حذف صلاحيات الدور أولاً
            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .ToListAsync();
            _context.RolePermissions.RemoveRange(rolePermissions);
            await _context.SaveChangesAsync();

            // حذف الدور
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"تم حذف الدور '{role.Name}' بنجاح.";
            }
            else
            {
                TempData["Error"] = "حدث خطأ أثناء حذف الدور.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        // عرض مستخدمي الدور
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Users(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            var allUsers = await _userManager.Users.OrderBy(u => u.FullName).ToListAsync();

            var model = new RoleUsersViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name ?? "",
                UsersInRole = usersInRole.ToList(),
                AllUsers = allUsers
            };

            return View(model);
        }

        // ══════════════════════════════════════════════════════
        // إضافة/إزالة مستخدم من الدور
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToRole(string roleId, string userId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var user = await _userManager.FindByIdAsync(userId);

            if (role == null || user == null) return NotFound();

            if (!await _userManager.IsInRoleAsync(user, role.Name!))
            {
                await _userManager.AddToRoleAsync(user, role.Name!);
                TempData["Success"] = $"تم إضافة '{user.FullName}' للدور '{role.Name}'.";
            }
            else
            {
                TempData["Error"] = "المستخدم موجود في هذا الدور بالفعل.";
            }

            return RedirectToAction(nameof(Users), new { id = roleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromRole(string roleId, string userId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var user = await _userManager.FindByIdAsync(userId);

            if (role == null || user == null) return NotFound();

            // منع إزالة Admin من دور Admin
            if (role.Name == "Admin" && user.Email == "admin@erp.com")
            {
                TempData["Error"] = "لا يمكن إزالة المدير العام من دور الأدمن.";
                return RedirectToAction(nameof(Users), new { id = roleId });
            }

            await _userManager.RemoveFromRoleAsync(user, role.Name!);
            TempData["Success"] = $"تم إزالة '{user.FullName}' من الدور '{role.Name}'.";

            return RedirectToAction(nameof(Users), new { id = roleId });
        }
    }
}