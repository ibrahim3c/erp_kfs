using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class RetirementController : Controller
    {
        public IActionResult Pending()
        {
            return View();
        }

        // ملفات تم تسليمها (Files)
        public IActionResult Files()
        {
            return View();
        }
    }
}