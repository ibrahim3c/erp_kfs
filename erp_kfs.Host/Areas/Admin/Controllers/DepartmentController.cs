using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using erp_kfs.Host.Models;

namespace erp_kfs.Host.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // تأمين الكنترولر للمديرين فقط
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ══════════════════════════════════════════════════════
        // 1. عرض الهيكل الإداري (Index)
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            // جلب الإدارات العامة (التي ليس لها أب) مع الإدارات الفرعية التابعة لها والمديرين
            var departments = await _context.Departments
                .Include(d => d.Manager)
                .Include(d => d.Children)
                    .ThenInclude(c => c.Manager)
                .Where(d => d.ParentId == null) // الإدارات الرئيسية فقط
                .OrderBy(d => d.Name)
                .ToListAsync();

            // جلب قائمة الموظفين لاستخدامها في الـ Modal الخاص بتعيين المدير
            ViewBag.Employees = await _context.EmployeeAdmins
                .Where(e => !e.IsTerminated)
                .OrderBy(e => e.FirstName)
                .ToListAsync();

            return View(departments); // إرسال المتغير departments إلى الـ View
        }

        // ══════════════════════════════════════════════════════
        // 2. إنشاء إدارة جديدة (Create)
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Create()
        {
            // إرسال الإدارات العامة فقط للـ View لاختيار الإدارة الأم (إذا كان النوع فرعي)
            ViewBag.GeneralDepartments = await _context.Departments
                .Where(d => d.Type == "General")
                .OrderBy(d => d.Name)
                .ToListAsync();

            return View(new Department());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                // توليد ID جديد
                model.Id = Guid.NewGuid().ToString();

                // إذا كان النوع "General"، يجب التأكد من مسح الـ ParentId
                if (model.Type == "General")
                {
                    model.ParentId = null;
                }

                _context.Departments.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"تم إنشاء الإدارة '{model.Name}' بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            // في حالة وجود خطأ، إعادة تحميل قائمة الإدارات العامة
            ViewBag.GeneralDepartments = await _context.Departments
                .Where(d => d.Type == "General")
                .OrderBy(d => d.Name)
                .ToListAsync();

            return View(model);
        }

        // ══════════════════════════════════════════════════════
        // 3. تعديل بيانات الإدارة (Edit)
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            ViewBag.Employees = await _context.EmployeeAdmins
                .Where(e => !e.IsTerminated)
                .OrderBy(e => e.FirstName)
                .ToListAsync();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Department model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // جلب الإدارة الأصلية من الداتابيز لتحديث الحقول المسموحة فقط
                    var existingDept = await _context.Departments.FindAsync(id);
                    if (existingDept == null) return NotFound();

                    existingDept.Name = model.Name;
                    existingDept.ManagerId = model.ManagerId;

                    // تحديث قاعدة البيانات
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "تم تعديل بيانات الإدارة بنجاح.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(model.Id)) return NotFound();
                    else throw;
                }
            }

            // في حالة فشل التحقق
            ViewBag.Employees = await _context.EmployeeAdmins
                .Where(e => !e.IsTerminated)
                .OrderBy(e => e.FirstName)
                .ToListAsync();

            return View(model);
        }

        // ══════════════════════════════════════════════════════
        // 4. تعيين مدير (Assign Manager - Ajax/Modal Action)
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignManager(string departmentId, string managerId)
        {
            if (string.IsNullOrEmpty(departmentId)) return BadRequest();

            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null) return NotFound();

            // تعيين المدير أو إزالته (إذا كان managerId فارغاً)
            department.ManagerId = string.IsNullOrEmpty(managerId) ? null : managerId;

            await _context.SaveChangesAsync();

            string managerName = string.IsNullOrEmpty(managerId) ? "تمت إزالة المدير" : "تم تعيين المدير";
            TempData["Success"] = $"{managerName} لإدارة '{department.Name}' بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        // Helper Method
        // ══════════════════════════════════════════════════════
        private bool DepartmentExists(string id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}