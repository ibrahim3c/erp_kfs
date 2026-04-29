using ERP_KFS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Dtos.OrgUnit;
using Organization.Application.Dtos.OrgUnitType;
using Organization.Application.IServices;
using Geography.Application.IServices;
using System.Diagnostics;
using Geography.Application.Dtos.Governorate;

namespace ERP_KFS_MVC.Areas.Organization.Controllers
{
    [Area("Organization")]
    public class OrgUnitsController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly IGeographyService _geographyService;

        public OrgUnitsController(IOrganizationService organizationService, IGeographyService geographyService)
        {
            _organizationService = organizationService;
            _geographyService = geographyService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllOrgUnitsAsync();

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value ?? new List<OrgUnitDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _organizationService.GetOrgUnitByIdAsync(id);

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

        public async Task<IActionResult> Create()
        {
            // تحميل القوائم المنسدلة
            var dropdownError = await PopulateDropdownsAsync();
            if (dropdownError != null) return dropdownError; // توجيه لصفحة الخطأ لو فشل التحميل

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrgUnitDto dto)
        {
            if (!ModelState.IsValid)
            {
                // إعادة تحميل القوائم المنسدلة في حالة وجود خطأ في الإدخال
                var dropdownError = await PopulateDropdownsAsync();
                if (dropdownError != null) return dropdownError;

                return View(dto);
            }

            var result = await _organizationService.CreateOrgUnitAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم إنشاء الوحدة التنظيمية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _organizationService.GetOrgUnitByIdAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            // تحميل القوائم المنسدلة لصفحة التعديل
            var dropdownError = await PopulateDropdownsAsync();
            if (dropdownError != null) return dropdownError;

            var dto = new UpdateOrgUnitDto(
                result.Value!.Id,
                result.Value.Name,
                result.Value.Code,
                result.Value.OrgUnitTypeId,
                result.Value.ParentId,
                result.Value.GovernorateId);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateOrgUnitDto dto)
        {
            if (!ModelState.IsValid)
            {
                // إعادة تحميل القوائم المنسدلة في حالة وجود خطأ في الإدخال
                var dropdownError = await PopulateDropdownsAsync();
                if (dropdownError != null) return dropdownError;

                return View(dto);
            }

            var result = await _organizationService.UpdateOrgUnitAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم تحديث الوحدة التنظيمية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _organizationService.DeleteOrgUnitAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم حذف الوحدة التنظيمية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        // =================================================================================
        // دالة مساعدة (Helper Method) لتحميل جميع القوائم المنسدلة لتجنب تكرار الكود
        // =================================================================================
        private async Task<IActionResult?> PopulateDropdownsAsync()
        {
            var typesResult = await _organizationService.GetAllOrgUnitTypesAsync();

            // لو فشل تحميل الأنواع (لأنها ضرورية)، نرجع صفحة Error
            if (typesResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = typesResult.Error.Code,
                    ErrorMessage = typesResult.Error.Name
                });
            }

            var unitsResult = await _organizationService.GetAllOrgUnitsAsync();
            var govResult = await _geographyService.GetAllGovernoratesAsync();

            ViewBag.OrgUnitTypes = typesResult.Value ?? new List<OrgUnitTypeDto>();
            ViewBag.ParentUnits = unitsResult.Value?.Where(u => u.IsActive).ToList() ?? new List<OrgUnitDto>();
            ViewBag.Governorates = govResult.Value ?? new List<GovernorateDto>();

            return null; 
        }
    }
}