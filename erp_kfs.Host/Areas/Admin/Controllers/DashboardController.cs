using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using MyERP.Web.Areas.Admin.Models;

namespace MyERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalEmployees = await _context.Employees.CountAsync(),
                TotalDepartments = await _context.Departments.CountAsync(),
                PendingLeaveRequests = await _context.LeaveRequests.CountAsync(lr => lr.Status == "PendingManager"),
                PendingHRRequests = await _context.LeaveRequests.CountAsync(lr => lr.Status == "PendingHR"),
                DepartmentsWithoutManagers = await _context.Departments.CountAsync(d => d.ManagerId == null)
            };
            return View(model);
        }
    }
}