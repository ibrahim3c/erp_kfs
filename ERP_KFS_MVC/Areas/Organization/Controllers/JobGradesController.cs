using ERP_KFS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Dtos.JobGrade;
using Organization.Application.IServices;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Organization.Controllers
{
    [Area("Organization")]
    public class JobGradesController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public JobGradesController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllJobGradesAsync();
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            return View(result.Value ?? new List<JobGradeDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _organizationService.GetJobGradeByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            return View(result.Value);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobGradeDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _organizationService.CreateJobGradeAsync(dto);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            TempData["Success"] = "تم إنشاء الدرجة الوظيفية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _organizationService.GetJobGradeByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            var dto = new UpdateJobGradeDto(result.Value.Id, result.Value.Code, result.Value.Name, result.Value.GradeLevel, result.Value.Description, result.Value.YearsNo);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateJobGradeDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _organizationService.UpdateJobGradeAsync(dto);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            TempData["Success"] = "تم تحديث الدرجة الوظيفية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _organizationService.DeleteJobGradeAsync(id);
            if (result.IsFailure)
                TempData["Error"] = result.Error.Name;
            else
                TempData["Success"] = "تم حذف الدرجة ال��ظيفية بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}