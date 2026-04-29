using ERP_KFS_MVC.Models;
using Geography.Application.Dtos.CityCenter;
using Geography.Application.Dtos.Governorate;
using Geography.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Geography.Controllers
{
    [Area("Geography")]
    public class CityCentersController : Controller
    {
        private readonly IGeographyService _geographyService;

        public CityCentersController(IGeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _geographyService.GetAllCityCentersAsync();

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value ?? new List<CityCenterDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _geographyService.GetCityCenterByIdAsync(id);

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
            var govResult = await _geographyService.GetAllGovernoratesAsync();

            if (govResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = govResult.Error.Code,
                    ErrorMessage = govResult.Error.Name
                });
            }

            ViewBag.Governorates = govResult.Value ?? new List<GovernorateDto>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCityCenterDto dto)
        {
            if (!ModelState.IsValid)
            {
                var govResult = await _geographyService.GetAllGovernoratesAsync();

                if (govResult.IsFailure)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorCode = govResult.Error.Code,
                        ErrorMessage = govResult.Error.Name
                    });
                }

                ViewBag.Governorates = govResult.Value ?? new List<GovernorateDto>();
                return View(dto);
            }

var result = await _geographyService.CreateCityCenterAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم إنشاء المركز بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _geographyService.GetCityCenterByIdAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            var govResult = await _geographyService.GetAllGovernoratesAsync();

            if (govResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = govResult.Error.Code,
                    ErrorMessage = govResult.Error.Name
                });
            }

            var dto = new UpdateCityCenterDto(
                result.Value!.Id,
                result.Value.GovernorateId,
                result.Value.Name,
                result.Value.Type);

            ViewBag.Governorates = govResult.Value ?? new List<GovernorateDto>();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCityCenterDto dto)
        {
            if (!ModelState.IsValid)
            {
                var govResult = await _geographyService.GetAllGovernoratesAsync();

                if (govResult.IsFailure)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorCode = govResult.Error.Code,
                        ErrorMessage = govResult.Error.Name
                    });
                }

                ViewBag.Governorates = govResult.Value ?? new List<GovernorateDto>();
                return View(dto);
            }

var result = await _geographyService.UpdateCityCenterAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم تحديث المركز بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
var result = await _geographyService.DeleteCityCenterAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم حذف المركز بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}