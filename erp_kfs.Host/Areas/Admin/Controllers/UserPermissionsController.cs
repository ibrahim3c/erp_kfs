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
    public class UserPermissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserPermissionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ══════════════════════════════════════════════════════
        // قائمة الموظفين (Index)
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userManager.Users
                    .Where(u => u.Email != "admin@erp.com" && u.Email != "recruiter@erp.com")
                    .OrderBy(u => u.FullName)
                    .ToListAsync();

                var taskCounts = await _context.UserPermissions
                    .Where(up => users.Select(u => u.Id).Contains(up.UserId))
                    .GroupBy(up => up.UserId)
                    .ToDictionaryAsync(g => g.Key, g => g.Count());

                ViewBag.UserTaskCounts = taskCounts;
                return View(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Index Error: {ex.Message}");
                TempData["Error"] = "حدث خطأ أثناء تحميل قائمة الموظفين.";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        // ══════════════════════════════════════════════════════
        // اختيار موظف (SelectEmployee)
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> SelectEmployee()
        {
            var users = await _userManager.Users
                .Where(u => u.Email != "admin@erp.com" && u.Email != "recruiter@erp.com")
                .OrderBy(u => u.FullName)
                .ToListAsync();

            return View(users);
        }

        // ══════════════════════════════════════════════════════
        // تعيين صلاحيات لمستخدم (AssignToUser)
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> AssignToUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var permissions = await _context.Permissions
                .OrderBy(p => p.Category)
                .ThenBy(p => p.DisplayName)
                .ToListAsync();

            var userPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.PermissionId)
                .ToListAsync();

            var model = new AssignUserPermissionsViewModel
            {
                UserId = userId,
                UserName = user.FullName,
                Permissions = permissions,
                SelectedPermissionIds = userPermissions
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToUser(AssignUserPermissionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Permissions = await _context.Permissions.ToListAsync();
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.UserPermissions
                    .Where(up => up.UserId == model.UserId)
                    .ToListAsync();
                _context.UserPermissions.RemoveRange(existing);

                if (model.SelectedPermissionIds?.Any() == true)
                {
                    foreach (var permId in model.SelectedPermissionIds)
                    {
                        _context.UserPermissions.Add(new UserPermission
                        {
                            UserId = model.UserId,
                            PermissionId = permId
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = $"تم تحديث صلاحيات '{model.UserName}' بنجاح.";
                return RedirectToAction(nameof(SelectEmployee));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ AssignToUser Error: {ex.Message}");
                TempData["Error"] = "حدث خطأ أثناء حفظ الصلاحيات.";
                model.Permissions = await _context.Permissions.ToListAsync();
                return View(model);
            }
        }

        // ══════════════════════════════════════════════════════
        // تعيين مهام (AssignTasks)
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> AssignTasks(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound();

                var permissions = await _context.Permissions
                    .OrderBy(p => p.Category)
                    .ThenBy(p => p.DisplayName)
                    .ToListAsync();

                var userTasks = await _context.UserPermissions
                    .Where(up => up.UserId == userId)
                    .Select(up => up.PermissionId)
                    .ToListAsync();

                var model = new AssignTasksViewModel
                {
                    UserId = userId,
                    UserName = user.FullName,
                    Permissions = permissions,
                    SelectedTaskIds = userTasks.ToArray()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ AssignTasks Error: {ex.Message}");
                TempData["Error"] = "حدث خطأ أثناء تحميل بيانات المهام.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTasks(AssignTasksViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Permissions = await _context.Permissions.ToListAsync();
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.UserPermissions
                    .Where(up => up.UserId == model.UserId)
                    .ToListAsync();
                _context.UserPermissions.RemoveRange(existing);

                if (model.SelectedTaskIds?.Any() == true)
                {
                    foreach (var taskId in model.SelectedTaskIds)
                    {
                        _context.UserPermissions.Add(new UserPermission
                        {
                            UserId = model.UserId,
                            PermissionId = taskId
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = $"تم تعيين المهام لـ {model.UserName} بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ AssignTasks POST Error: {ex.Message}");
                TempData["Error"] = "حدث خطأ أثناء حفظ المهام.";
                model.Permissions = await _context.Permissions.ToListAsync();
                return View(model);
            }
        }
    }
}