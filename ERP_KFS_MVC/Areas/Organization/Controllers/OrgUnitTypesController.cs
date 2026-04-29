using ERP_KFS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Dtos.OrgUnitType;
using Organization.Application.IServices;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Organization.Controllers
{
    [Area("Organization")]
    public class OrgUnitTypesController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public OrgUnitTypesController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllOrgUnitTypesAsync();

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value ?? new List<OrgUnitTypeDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _organizationService.GetOrgUnitTypeByIdAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrgUnitTypeDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

var result = await _organizationService.CreateOrgUnitTypeAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم إنشاء نوع الوحدة التنظيمية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _organizationService.GetOrgUnitTypeByIdAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            var dto = new UpdateOrgUnitTypeDto(
                result.Value!.Id,
                result.Value.Code,
                result.Value.Name,
                result.Value.LevelOrder,
                result.Value.CanHaveChild);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateOrgUnitTypeDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

var result = await _organizationService.UpdateOrgUnitTypeAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم تحديث نوع الوحدة التنظيمية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
var result = await _organizationService.DeleteOrgUnitTypeAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم حذف نوع الوحدة التنظيمية بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}