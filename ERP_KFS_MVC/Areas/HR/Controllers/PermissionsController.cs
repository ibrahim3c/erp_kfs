using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class PermissionsController : Controller
    {
        // صفحة سجل الأذونات والتأخير
        public IActionResult Index()
        {
            return View();
        }
    }
}