using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using MyERP.Web.Models;
using MyERP.Web.Areas.Admin.Models;

namespace MyERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PermissionsManageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PermissionsManageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ══════════════════════════════════════════════════════
        // عرض كل الصلاحيات
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            var permissions = await _context.Permissions
                .OrderBy(p => p.Category)
                .ThenBy(p => p.DisplayName)
                .ToListAsync();

            // عدد الأدوار والمستخدمين لكل صلاحية
            var permStats = new Dictionary<string, PermissionStatsViewModel>();
            foreach (var perm in permissions)
            {
                var roleCount = await _context.RolePermissions.CountAsync(rp => rp.PermissionId == perm.Id);
                var userCount = await _context.UserPermissions.CountAsync(up => up.PermissionId == perm.Id);
                permStats[perm.Id] = new PermissionStatsViewModel
                {
                    RoleCount = roleCount,
                    UserCount = userCount
                };
            }

            ViewBag.PermStats = permStats;
            ViewBag.Categories = permissions.Select(p => p.Category).Distinct().OrderBy(c => c).ToList();
            return View(permissions);
        }

        // ══════════════════════════════════════════════════════
        // إنشاء صلاحية جديدة
        // ══════════════════════════════════════════════════════
        public IActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePermissionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategories();
                return View(model);
            }

            // التحقق من عدم تكرار الاسم
            if (await _context.Permissions.AnyAsync(p => p.Name == model.Name))
            {
                ModelState.AddModelError("Name", "هذا الكود مستخدم بالفعل.");
                ViewBag.Categories = GetCategories();
                return View(model);
            }

            var permission = new Permission
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                DisplayName = model.DisplayName,
                Category = string.IsNullOrEmpty(model.NewCategory) ? model.Category : model.NewCategory,
                LegalDescription = model.LegalDescription ?? "",
                LegalReference = model.LegalReference ?? ""
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"تم إنشاء الصلاحية '{model.DisplayName}' بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        // تعديل صلاحية
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Edit(string id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return NotFound();

            ViewBag.Categories = GetCategories();

            var model = new EditPermissionViewModel
            {
                Id = permission.Id,
                Name = permission.Name,
                DisplayName = permission.DisplayName,
                Category = permission.Category,
                LegalDescription = permission.LegalDescription,
                LegalReference = permission.LegalReference
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPermissionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategories();
                return View(model);
            }

            var permission = await _context.Permissions.FindAsync(model.Id);
            if (permission == null) return NotFound();

            // التحقق من عدم تكرار الاسم مع صلاحية أخرى
            if (permission.Name != model.Name &&
                await _context.Permissions.AnyAsync(p => p.Name == model.Name))
            {
                ModelState.AddModelError("Name", "هذا الكود مستخدم بالفعل.");
                ViewBag.Categories = GetCategories();
                return View(model);
            }

            permission.Name = model.Name;
            permission.DisplayName = model.DisplayName;
            permission.Category = string.IsNullOrEmpty(model.NewCategory) ? model.Category : model.NewCategory;
            permission.LegalDescription = model.LegalDescription ?? "";
            permission.LegalReference = model.LegalReference ?? "";

            await _context.SaveChangesAsync();

            TempData["Success"] = $"تم تعديل الصلاحية '{model.DisplayName}' بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        // حذف صلاحية
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return NotFound();

            // التحقق من الاستخدام
            var roleUsage = await _context.RolePermissions.CountAsync(rp => rp.PermissionId == id);
            var userUsage = await _context.UserPermissions.CountAsync(up => up.PermissionId == id);

            if (roleUsage > 0 || userUsage > 0)
            {
                TempData["Error"] = $"لا يمكن حذف الصلاحية '{permission.DisplayName}' لأنها مُعيّنة لـ {roleUsage} دور و {userUsage} مستخدم.";
                return RedirectToAction(nameof(Index));
            }

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"تم حذف الصلاحية '{permission.DisplayName}' بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        // دوال مساعدة
        // ══════════════════════════════════════════════════════
        private List<string> GetCategories()
        {
            return new List<string>
            {
                "General",
                "Recruitment",
                "HR",
                "Departments",
                "Leadership",
                "Employee",
                "Attendance",
                "Payroll"
            };
        }
    }
}