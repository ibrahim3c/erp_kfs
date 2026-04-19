using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using MyERP.Web.Models;
using System.Globalization;

namespace erp_kfs.Host.Areas.Admin.Controllers
{
[Area("Admin")]
[Authorize(Policy = "Permission.Leadership.Manage")]
    public class LeadershipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeadershipController(ApplicationDbContext context)
        {
            _context = context;
        }

 public async Task<IActionResult> Index()
{
    var assignments = await _context.LeadershipAssignments
        .Include(a => a.Position)
            .ThenInclude(p => p.Department)
        .Include(a => a.Employee)
        .Where(a => a.IsCurrent && a.Position != null) // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
        .OrderBy(a => a.Position.Level)
        .ToListAsync();

    return View(assignments);
}

        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _context.GlobalLeadershipPositions.OrderBy(p => p.Level).ToListAsync();
            ViewBag.Employees = await _context.EmployeeAdmins
                .Where(e => !e.IsTerminated && e.HireDate.HasValue)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeadershipAssignment model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = await _context.GlobalLeadershipPositions.OrderBy(p => p.Level).ToListAsync();
                ViewBag.Employees = await _context.EmployeeAdmins.Where(e => !e.IsTerminated).OrderBy(e => e.FirstName).ToListAsync();
                return View(model);
            }

            // إنهاء التكليف القديم لنفس الموظف
            var oldAssignment = await _context.LeadershipAssignments
                .FirstOrDefaultAsync(a => a.EmployeeId == model.EmployeeId && a.IsCurrent);
            if (oldAssignment != null)
            {
                oldAssignment.IsCurrent = false;
                oldAssignment.EndDate = DateTime.Now;
            }

            // إنهاء التكليف القديم لنفس المنصب
            var oldPositionAssignment = await _context.LeadershipAssignments
                .FirstOrDefaultAsync(a => a.PositionId == model.PositionId && a.IsCurrent);
            if (oldPositionAssignment != null && oldPositionAssignment.EmployeeId != model.EmployeeId)
            {
                oldPositionAssignment.IsCurrent = false;
                oldPositionAssignment.EndDate = DateTime.Now;
            }

            model.Id = Guid.NewGuid().ToString();
            model.IsCurrent = true;
            model.AssignedDate = DateTime.Now;
            model.HijriDate = GetHijriDate(DateTime.Now);

            _context.LeadershipAssignments.Add(model);

            // ربط الموظف بالإدارة لو المنصب مناسب
            var position = await _context.GlobalLeadershipPositions.FindAsync(model.PositionId);
            if (position != null && position.Title != "Governor" && position.Title != "ChiefSecretary")
            {
                if (!string.IsNullOrEmpty(position.DepartmentId))
                {
                    var department = await _context.Departments.FindAsync(position.DepartmentId);
                    if (department != null)
                    {
                        department.ManagerId = model.EmployeeId;
                    }
                }
            }

            await _context.SaveChangesAsync();

            // ✅ حمّل البيانات للرسالة
            var emp = await _context.EmployeeAdmins.FindAsync(model.EmployeeId);
            var pos = await _context.GlobalLeadershipPositions.FindAsync(model.PositionId);
            TempData["Success"] = $"تم تعيين {emp?.FullName} في منصب {pos?.DisplayName} بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var assignment = await _context.LeadershipAssignments
                .Include(a => a.Position)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null) return NotFound();

            ViewBag.Positions = await _context.GlobalLeadershipPositions.OrderBy(p => p.Level).ToListAsync();
            ViewBag.Employees = await _context.EmployeeAdmins.Where(e => !e.IsTerminated).OrderBy(e => e.FirstName).ToListAsync();
            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, LeadershipAssignment model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Positions = await _context.GlobalLeadershipPositions.OrderBy(p => p.Level).ToListAsync();
                ViewBag.Employees = await _context.EmployeeAdmins.Where(e => !e.IsTerminated).OrderBy(e => e.FirstName).ToListAsync();
                return View(model);
            }

            // ✅ حمّل من الـ DB وعدّل الحقول المسموح بيها بس
            var existing = await _context.LeadershipAssignments.FindAsync(id);
            if (existing == null) return NotFound();

            existing.EmployeeId = model.EmployeeId;
            existing.PositionId = model.PositionId;
            existing.IsCurrent = model.IsCurrent;
            existing.EndDate = model.EndDate;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تعديل التكليف القيادي بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.LeadershipAssignments.Any(e => e.Id == model.Id))
                    return NotFound();
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndAssignment(string id)
        {
            var assignment = await _context.LeadershipAssignments.FindAsync(id);
            if (assignment == null || !assignment.IsCurrent) return NotFound();

            assignment.IsCurrent = false;
            assignment.EndDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إنهاء التكليف القيادي بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        private static string GetHijriDate(DateTime date)
        {
            try
            {
                var hijriCalendar = new UmAlQuraCalendar();
                return $"{hijriCalendar.GetDayOfMonth(date)}/{hijriCalendar.GetMonth(date)}/{hijriCalendar.GetYear(date)}";
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}