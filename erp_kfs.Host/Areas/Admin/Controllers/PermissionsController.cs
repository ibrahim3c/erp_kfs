using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using MyERP.Web.Models;
using erp_kfs.Host.Models;

namespace MyERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PermissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionsController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
                return View(roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ PermissionsController.Index Error: {ex.Message}");
                TempData["Error"] = "حدث خطأ أثناء تحميل قائمة الأدوار.";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> Assign(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null) return NotFound();

                var permissions = await _context.Permissions
                    .OrderBy(p => p.Category)
                    .ThenBy(p => p.DisplayName)
                    .ToListAsync();

                var rolePermissions = await _context.RolePermissions
                    .Where(rp => rp.RoleId == roleId)
                    .Select(rp => rp.PermissionId)
                    .ToListAsync();

                var model = new AssignPermissionsViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name ?? "",
                    Permissions = permissions,
                    SelectedPermissionIds = rolePermissions.ToArray()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Assign Error: {ex.Message}");
                TempData["Error"] = "حدث خطأ أثناء تحميل بيانات الصلاحيات.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignPermissionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Permissions = await _context.Permissions.OrderBy(p => p.DisplayName).ToListAsync();
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingPermissions = await _context.RolePermissions
                    .Where(rp => rp.RoleId == model.RoleId)
                    .ToListAsync();

                _context.RolePermissions.RemoveRange(existingPermissions);

                if (model.SelectedPermissionIds?.Length > 0)
                {
                    var newPermissions = model.SelectedPermissionIds.Select(pid => new RolePermission
                    {
                        RoleId = model.RoleId,
                        PermissionId = pid
                    });
                    _context.RolePermissions.AddRange(newPermissions);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = $"تم تحديث صلاحيات الدور '{model.RoleName}' بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ Assign POST Error: {ex.Message}");
                TempData["Error"] = "حدث خطأ أثناء حفظ الصلاحيات.";
                model.Permissions = await _context.Permissions.ToListAsync();
                return View(model);
            }
        }
    }
}