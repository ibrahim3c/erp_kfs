using ERP_KFS_MVC.Models;
using Geography.Application.Dtos.LocalUnit;
using Geography.Application.Dtos.Village;
using Geography.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Geography.Controllers
{
    [Area("Geography")]
    public class VillagesController : Controller
    {
        private readonly IGeographyService _geographyService;

        public VillagesController(IGeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _geographyService.GetAllVillagesAsync();

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value ?? new List<VillageDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _geographyService.GetVillageByIdAsync(id);

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
            var luResult = await _geographyService.GetAllLocalUnitsAsync();

            if (luResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = luResult.Error.Code,
                    ErrorMessage = luResult.Error.Name
                });
            }

            ViewBag.LocalUnits = luResult.Value ?? new List<LocalUnitDto>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVillageDto dto)
        {
            if (!ModelState.IsValid)
            {
                var luResult = await _geographyService.GetAllLocalUnitsAsync();

                if (luResult.IsFailure)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorCode = luResult.Error.Code,
                        ErrorMessage = luResult.Error.Name
                    });
                }

                ViewBag.LocalUnits = luResult.Value ?? new List<LocalUnitDto>();
                return View(dto);
            }

var result = await _geographyService.CreateVillageAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم إنشاء القرية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _geographyService.GetVillageByIdAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            var luResult = await _geographyService.GetAllLocalUnitsAsync();

            if (luResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = luResult.Error.Code,
                    ErrorMessage = luResult.Error.Name
                });
            }

            var dto = new UpdateVillageDto(
                result.Value!.Id,
                result.Value.LocalUnitId,
                result.Value.Name);

            ViewBag.LocalUnits = luResult.Value ?? new List<LocalUnitDto>();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateVillageDto dto)
        {
            if (!ModelState.IsValid)
            {
                var luResult = await _geographyService.GetAllLocalUnitsAsync();

                if (luResult.IsFailure)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorCode = luResult.Error.Code,
                        ErrorMessage = luResult.Error.Name
                    });
                }

                ViewBag.LocalUnits = luResult.Value ?? new List<LocalUnitDto>();
                return View(dto);
            }

var result = await _geographyService.UpdateVillageAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم تحديث القرية بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
var result = await _geographyService.DeleteVillageAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم حذف القرية بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}