using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class LegalController : Controller
    {
        // صفحة متابعة تنفيذ الأحكام
        public IActionResult Rulings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExecuteRuling(IFormCollection form)
        {
            return RedirectToAction(nameof(Rulings));
        }
    }
}