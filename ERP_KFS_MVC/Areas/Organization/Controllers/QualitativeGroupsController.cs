using ERP_KFS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Dtos.QualitativeGroup;
using Organization.Application.IServices;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Organization.Controllers
{
    [Area("Organization")]
    public class QualitativeGroupsController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public QualitativeGroupsController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllQualitativeGroupsAsync();
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            return View(result.Value ?? new List<QualitativeGroupDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _organizationService.GetQualitativeGroupByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            return View(result.Value);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQualitativeGroupDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _organizationService.CreateQualitativeGroupAsync(dto);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            TempData["Success"] = "تم إنشاء المجموعة الوظيفية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _organizationService.GetQualitativeGroupByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            var dto = new UpdateQualitativeGroupDto(result.Value.Id, result.Value.Code, result.Value.Name, result.Value.Description);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateQualitativeGroupDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _organizationService.UpdateQualitativeGroupAsync(dto);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            TempData["Success"] = "تم تحديث المجموعة الوظيفية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _organizationService.DeleteQualitativeGroupAsync(id);
            if (result.IsFailure)
                TempData["Error"] = result.Error.Name;
            else
                TempData["Success"] = "تم حذف المجموعة الوظيفية بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}