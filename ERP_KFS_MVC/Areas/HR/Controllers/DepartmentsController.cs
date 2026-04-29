using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Modal for Adding/Editing Department
        [HttpPost]
        public IActionResult Save(IFormCollection form)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}