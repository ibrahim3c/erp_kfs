using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class FundsController : Controller
    {
        // صفحة إدارة الصناديق
        public IActionResult Fellowship()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMember(IFormCollection form)
        {
            return RedirectToAction(nameof(Fellowship));
        }
    }
}