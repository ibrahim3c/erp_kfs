using ERP_KFS_MVC.Models;
using Geography.Application.Dtos.CityCenter;
using Geography.Application.Dtos.LocalUnit;
using Geography.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Geography.Controllers
{
    [Area("Geography")]
    public class LocalUnitsController : Controller
    {
        private readonly IGeographyService _geographyService;

        public LocalUnitsController(IGeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _geographyService.GetAllLocalUnitsAsync();

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value ?? new List<LocalUnitDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _geographyService.GetLocalUnitByIdAsync(id);

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
            var ccResult = await _geographyService.GetAllCityCentersAsync();

            if (ccResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = ccResult.Error.Code,
                    ErrorMessage = ccResult.Error.Name
                });
            }

            ViewBag.CityCenters = ccResult.Value ?? new List<CityCenterDto>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLocalUnitDto dto)
        {
            if (!ModelState.IsValid)
            {
                var ccResult = await _geographyService.GetAllCityCentersAsync();

                if (ccResult.IsFailure)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorCode = ccResult.Error.Code,
                        ErrorMessage = ccResult.Error.Name
                    });
                }

                ViewBag.CityCenters = ccResult.Value ?? new List<CityCenterDto>();
                return View(dto);
            }

var result = await _geographyService.CreateLocalUnitAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم إنشاء الوحدة المحلية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _geographyService.GetLocalUnitByIdAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            var ccResult = await _geographyService.GetAllCityCentersAsync();

            if (ccResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = ccResult.Error.Code,
                    ErrorMessage = ccResult.Error.Name
                });
            }

            var dto = new UpdateLocalUnitDto(
                result.Value!.Id,
                result.Value.CityCenterId,
                result.Value.Name);

            ViewBag.CityCenters = ccResult.Value ?? new List<CityCenterDto>();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateLocalUnitDto dto)
        {
            if (!ModelState.IsValid)
            {
                var ccResult = await _geographyService.GetAllCityCentersAsync();

                if (ccResult.IsFailure)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorCode = ccResult.Error.Code,
                        ErrorMessage = ccResult.Error.Name
                    });
                }

                ViewBag.CityCenters = ccResult.Value ?? new List<CityCenterDto>();
                return View(dto);
            }

var result = await _geographyService.UpdateLocalUnitAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم تحديث الوحدة المحلية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
var result = await _geographyService.DeleteLocalUnitAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم حذف الوحدة المحلية بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}