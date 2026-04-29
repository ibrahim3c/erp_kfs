using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Roles");
        }
    }
}