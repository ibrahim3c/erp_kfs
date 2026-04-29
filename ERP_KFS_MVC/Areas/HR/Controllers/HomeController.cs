using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}