using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
