using ERP_KFS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Dtos.QualitativeGroup;
using Organization.Application.Dtos.FunctionalGroup;
using Organization.Application.IServices;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Organization.Controllers
{
    [Area("Organization")]
    public class FunctionalGroupsController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public FunctionalGroupsController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllFunctionalGroupsAsync();
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            return View(result.Value ?? new List<FunctionalGroupDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _organizationService.GetFunctionalGroupByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            return View(result.Value);
        }

        public async Task<IActionResult> Create()
        {
            var qgResult = await _organizationService.GetAllQualitativeGroupsAsync();
            ViewBag.QualitativeGroups = qgResult.Value ?? new List<QualitativeGroupDto>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFunctionalGroupDto dto)
        {
            if (!ModelState.IsValid)
            {
                var qgResult = await _organizationService.GetAllQualitativeGroupsAsync();
                ViewBag.QualitativeGroups = qgResult.Value ?? new List<QualitativeGroupDto>();
                return View(dto);
            }
            var result = await _organizationService.CreateFunctionalGroupAsync(dto);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            TempData["Success"] = "تم إنشاء المجموعة الوظيفية الفرعية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _organizationService.GetFunctionalGroupByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            var dto = new UpdateFunctionalGroupDto(result.Value.Id, result.Value.QualitativeGroupId, result.Value.Code, result.Value.Name, result.Value.Description);
            var qgResult = await _organizationService.GetAllQualitativeGroupsAsync();
            ViewBag.QualitativeGroups = qgResult.Value ?? new List<QualitativeGroupDto>();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateFunctionalGroupDto dto)
        {
            if (!ModelState.IsValid)
            {
                var qgResult = await _organizationService.GetAllQualitativeGroupsAsync();
                ViewBag.QualitativeGroups = qgResult.Value ?? new List<QualitativeGroupDto>();
                return View(dto);
            }
            var result = await _organizationService.UpdateFunctionalGroupAsync(dto);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            TempData["Success"] = "تم تحديث المجموعة الوظيفية الفرعية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _organizationService.DeleteFunctionalGroupAsync(id);
            if (result.IsFailure)
                TempData["Error"] = result.Error.Name;
            else
                TempData["Success"] = "تم حذف المجموعة الوظيفية الفرعية بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}