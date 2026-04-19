using Microsoft.AspNetCore.Mvc;

namespace erp_kfs.Host.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LeaveRequestsController : Controller
    {
        public IActionResult ManagerPending()
        {
            return View();
        }
        public IActionResult HRPending()
        {
            return View();
        }
    }
}
