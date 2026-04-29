using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class SecondmentsController : Controller
    {
        // صفحة الإعارات والندب
        public IActionResult Index()
        {
            return View();
        }

        // إنشاء إعارة جديدة
        public IActionResult Create()
        {
            return View();
        }
    }
}