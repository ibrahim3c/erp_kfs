using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class LeavesController : Controller
    {
        // صفحة الأجازات الاعتيادية والعارضة
        public IActionResult Regular()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRegularRequest(IFormCollection form)
        {
            // كود حفظ الطلب وخصمه من الرصيد
            return RedirectToAction(nameof(Regular));
        }
        // صفحة الأجازات المرضية
        public IActionResult Medical()
        {
            return View();
        }
        // الأجازات الخاصة (Special Leaves)
        public IActionResult Special()
        {
            return View();
        }
    }
}