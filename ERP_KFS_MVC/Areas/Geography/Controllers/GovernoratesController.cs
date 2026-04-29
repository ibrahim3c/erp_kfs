using ERP_KFS_MVC.Models;
using Geography.Application.Dtos.Governorate;
using Geography.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Geography.Controllers
{
    [Area("Geography")]
    public class GovernoratesController : Controller
    {
        private readonly IGeographyService _geographyService;

        public GovernoratesController(IGeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _geographyService.GetAllGovernoratesAsync();

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value ?? new List<GovernorateDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _geographyService.GetGovernorateByIdAsync(id);

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
        public async Task<IActionResult> Create(CreateGovernorateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

var result = await _geographyService.CreateGovernorateAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم إنشاء المحافظة بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _geographyService.GetGovernorateByIdAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            var dto = new UpdateGovernorateDto(
                result.Value!.Id,
                result.Value.Name,
                result.Value.Code);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateGovernorateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

var result = await _geographyService.UpdateGovernorateAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم تحديث المحافظة بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
var result = await _geographyService.DeleteGovernorateAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم حذف المحافظة بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}