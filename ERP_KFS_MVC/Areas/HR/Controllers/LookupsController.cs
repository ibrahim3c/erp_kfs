using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class LookupsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}