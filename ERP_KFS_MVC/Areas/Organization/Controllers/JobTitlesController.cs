using ERP_KFS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Dtos.FunctionalGroup;
using Organization.Application.Dtos.JobTitle;
using Organization.Application.IServices;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Organization.Controllers
{
    [Area("Organization")]
    public class JobTitlesController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public JobTitlesController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllJobTitlesAsync();
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            return View(result.Value ?? new List<JobTitleDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _organizationService.GetJobTitleByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            return View(result.Value);
        }

        public async Task<IActionResult> Create()
        {
            var fgResult = await _organizationService.GetAllFunctionalGroupsAsync();
            ViewBag.FunctionalGroups = fgResult.Value ?? new List<FunctionalGroupDto>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobTitleDto dto)
        {
            if (!ModelState.IsValid)
            {
                var fgResult = await _organizationService.GetAllFunctionalGroupsAsync();
                ViewBag.FunctionalGroups = fgResult.Value ?? new List<FunctionalGroupDto>();
                return View(dto);
            }
            var result = await _organizationService.CreateJobTitleAsync(dto);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            TempData["Success"] = "تم إنشاء المسمى الوظيفي بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _organizationService.GetJobTitleByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            var dto = new UpdateJobTitleDto(result.Value.Id, result.Value.FunctionalGroupId, result.Value.Code, result.Value.Name, result.Value.Description);
            var fgResult = await _organizationService.GetAllFunctionalGroupsAsync();
            ViewBag.FunctionalGroups = fgResult.Value ?? new List<FunctionalGroupDto>();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateJobTitleDto dto)
        {
            if (!ModelState.IsValid)
            {
                var fgResult = await _organizationService.GetAllFunctionalGroupsAsync();
                ViewBag.FunctionalGroups = fgResult.Value ?? new List<FunctionalGroupDto>();
                return View(dto);
            }
            var result = await _organizationService.UpdateJobTitleAsync(dto);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorCode = result.Error.Code, ErrorMessage = result.Error.Name });
            TempData["Success"] = "تم تحديث المسمى الوظيفي بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _organizationService.DeleteJobTitleAsync(id);
            if (result.IsFailure)
                TempData["Error"] = result.Error.Name;
            else
                TempData["Success"] = "تم حذف المسمى الوظيفي بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}