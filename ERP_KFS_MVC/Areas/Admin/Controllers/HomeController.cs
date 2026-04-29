using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_KFS_MVC.Areas.Admin.ViewModels;

namespace ERP_KFS_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account", new { area = "Admin", returnUrl = Url.Action("Index", "Home", new { area = "Admin" }) });
            }

            if (!User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                return RedirectToAction("AccessDenied", "Account", new { area = "Admin" });
            }

            var model = new AdminDashboardViewModel
            {
                TotalEmployees = 0,
                PendingLeaveRequests = 0,
                PendingHRRequests = 0,
                DepartmentsWithoutManagers = 0
            };
            return View(model);
        }
    }
}