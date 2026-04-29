using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class AbsenceController : Controller
    {
        // صفحة تصفية الغياب
        public IActionResult Settlement()
        {
            return View();
        }

        // تنفيذ التسوية
        [HttpPost]
        public IActionResult Settle(IFormCollection form)
        {
            // Code to update db (Deduct Balance OR Salary Penalty)
            return RedirectToAction(nameof(Settlement));
        }
    }
}