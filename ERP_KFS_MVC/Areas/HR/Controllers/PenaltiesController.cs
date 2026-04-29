using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class PenaltiesController : Controller
    {
        // الصفحة الرئيسية (السجل العام)
        public IActionResult Index()
        {
            return View();
        }

        // تسجيل جزاء جديد
        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}