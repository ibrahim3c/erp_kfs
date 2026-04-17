using HR.Application.Penalties.CreatePenalty;
using HR.Application.Penalties.DeletePenalty;
using HR.Application.Penalties.EditPenalty;
using HR.Application.Penalties.GetPenaltyList;
using HR.Domain.Penalties;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyERP.Web.Areas.HR.Controllers
{
    [Area("HR")]
    public class PenaltiesController : Controller
    {
        private readonly IMediator _mediator;

        public PenaltiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ══════════════════════════════════════════════════════
        //  الصفحة الرئيسية
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetPenaltyListQuery());

            if (result.IsFailure)
                return View("Error", result.Error);

            return View(result.Value);
        }

        // ══════════════════════════════════════════════════════
        //  تسجيل جزاء جديد
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Guid employeeId,
            string violationType,
            DateTime violationDate,
            string penaltyType,
            int deductionDays,
            string executionMonth,
            string decisionReference,
            string reasons,
            IFormFile? decisionFile)
        {
            string? filePath = await UploadFileAsync(decisionFile);
            var type = MapActionType(penaltyType);

            if (!TryParseExecutionMonth(executionMonth, out var execDate))
                return RedirectToAction(nameof(Index));

            var command = new CreatePenaltyCommand(
                employeeId, violationDate, type, violationType,
                deductionDays, execDate, decisionReference, reasons, filePath);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم اعتماد الجزاء بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        //  تعديل جزاء 
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            string violationType,
            DateTime violationDate,
            string penaltyType,
            int deductionDays,
            string executionMonth,
            string decisionReference,
            string reasons,
            IFormFile? decisionFile)
        {
            string? filePath = await UploadFileAsync(decisionFile);
            var type = MapActionType(penaltyType);

            if (!TryParseExecutionMonth(executionMonth, out var execDate))
                return RedirectToAction(nameof(Index));

            // 👈 بنبعت الأي دي وباقي الداتا للـ Command الخاص بالتعديل
            var command = new EditPenaltyCommand(
                id, violationDate, type, violationType,
                deductionDays, execDate, decisionReference, reasons, filePath);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تعديل الجزاء بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        //  حذف جزاء (بعد قبول تظلم)
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "رقم الجزاء غير صحيح.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _mediator.Send(new DeletePenaltyCommand(id));

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم حذف الجزاء بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        // ─── دوال مساعدة Helpers ──────────────────
        private async Task<string?> UploadFileAsync(IFormFile? file)
        {
            if (file is not { Length: > 0 }) return null;
            var folder = Path.Combine("wwwroot", "uploads", "penalties");
            Directory.CreateDirectory(folder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(folder, fileName);
            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return Path.Combine("uploads", "penalties", fileName);
        }

        private static PenaltyActionType MapActionType(string penaltyType) => penaltyType switch
        {
            "Warning" => PenaltyActionType.Warning,
            "Hold" => PenaltyActionType.Hold,
            "Postpone" => PenaltyActionType.Postpone,
            _ => PenaltyActionType.Deduct
        };

        private bool TryParseExecutionMonth(string executionMonth, out DateTime execDate)
        {
            if (!DateTime.TryParseExact(executionMonth, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out execDate))
            {
                TempData["ErrorMessage"] = "شهر التنفيذ غير صحيح.";
                return false;
            }
            return true;
        }
    }
}
