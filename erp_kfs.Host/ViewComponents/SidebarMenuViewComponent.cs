using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using MyERP.Web.Models;
using erp_kfs.Host.Models;

namespace MyERP.Web.ViewComponents
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SidebarMenuViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return View(new SidebarMenuViewModel());

            var employee = await _context.EmployeeAdmins
                .FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);

            bool isManager = false;
            int pendingLeaveRequests = 0;

            if (employee != null)
            {
                var managedDeptIds = await _context.Departments
                    .Where(d => d.ManagerId == employee.Id)
                    .Select(d => d.Id)
                    .ToListAsync();

                isManager = managedDeptIds.Any();

                if (isManager)
                {
                    var managedEmployeeIds = await _context.EmployeeAdmins
                        .Where(e => managedDeptIds.Contains(e.SelectedDepartmentId!))
                        .Select(e => e.Id)
                        .ToListAsync();

                    pendingLeaveRequests = await _context.LeaveRequests
                        .CountAsync(lr => managedEmployeeIds.Contains(lr.EmployeeId!)
                                       && lr.Status == "PendingManager");
                }
            }

            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

            var model = new SidebarMenuViewModel
            {
                IsManager = isManager,
                PendingLeaveRequestsCount = pendingLeaveRequests,
                UserRoles = userRoles
            };

            return View(model);
        }
    }

    public class SidebarMenuViewModel
    {
        public bool IsManager { get; set; }
        public int PendingLeaveRequestsCount { get; set; }
        public List<string> UserRoles { get; set; } = new();
    }
}