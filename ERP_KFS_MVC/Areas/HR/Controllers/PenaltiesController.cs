using HR.Application.Employees.GetAllEmployees;
using HR.Application.Penalties.CreatePenalty;
using HR.Application.Penalties.DeletePenalty;
using HR.Application.Penalties.EditPenalty;
using HR.Application.Penalties.GetPenaltyDetails;
using HR.Application.Penalties.GetPenaltyList;
using HR.Domain.Penalties;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Organization.Domain;
using System.Globalization;

namespace ERP_KFS_MVC.Areas.HR.Controllers
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

            var employees = await _mediator.Send(new GetAllEmployeesQuery());
            ViewBag.Employees = employees.Value ?? new List<EmployeeListResponse>();

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
            string executionMonth,    // من <input type="month">
            string decisionReference,
            string reasons,
            IFormFile? decisionFile)
        {
            // رفع الملف
            string? filePath = null;
            if (decisionFile is { Length: > 0 })
            {
                var folder = Path.Combine("wwwroot", "uploads", "penalties");
                Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(decisionFile.FileName)}";
                var fullPath = Path.Combine(folder, fileName);
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await decisionFile.CopyToAsync(stream);
                filePath = Path.Combine("uploads", "penalties", fileName);
            }

            // تحويل نوع الجزاء
            var type = penaltyType switch
            {
                "Warning" => PenaltyActionType.Warning,
                "Hold" => PenaltyActionType.Hold,
                "Postpone" => PenaltyActionType.Postpone,
                _ => PenaltyActionType.Deduct
            };

            // تحويل شهر التنفيذ
            if (!DateTime.TryParseExact(executionMonth, "yyyy-MM",
                    null, System.Globalization.DateTimeStyles.None,
                    out var execDate))
            {
                TempData["ErrorMessage"] = "شهر التنفيذ غير صحيح.";
                return RedirectToAction(nameof(Index));
            }

            var command = new CreatePenaltyCommand(
                employeeId, violationDate, type, violationType, deductionDays, execDate,
                decisionReference, reasons, filePath);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم اعتماد الجزاء بنجاح.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _mediator.Send(new GetPenaltyDetailsQuery(id));
            if (result.IsFailure)
                return View("Error", result.Error.Name);

            return View(result.Value);
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
        public async Task<IActionResult> Edit(
                Guid id,
                string violationType,
                string penaltyType,
                decimal? deductionDays,
                string executionMonth,
                string decisionReference,
                string? notes,
                IFormFile? decisionFile)
        {
            if (!DateTime.TryParseExact(executionMonth, "yyyy-MM",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var executionDate))
            {
                TempData["ErrorMessage"] = $"القيمة المستلمة: '{executionMonth}'";
                return RedirectToAction(nameof(Index));
            }

            // تحويل الـ string لـ Enum
            if (!Enum.TryParse<PenaltyActionType>(penaltyType, out var actionType))
            {
                TempData["ErrorMessage"] = "نوع الجزاء غير صحيح.";
                return RedirectToAction(nameof(Index));
            }

            // رفع المرفق لو موجود
            string? filePath = null;
            if (decisionFile is { Length: > 0 })
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads", "penalties");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(decisionFile.FileName)}";
                filePath = Path.Combine(uploadsFolder, fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await decisionFile.CopyToAsync(stream);
                filePath = Path.Combine("uploads", "penalties", fileName);
            }

            var command = new EditPenaltyCommand(
                PenaltyId: id,
                ViolationDate: DateTime.Today, // لو عندك حقل في الـ form ضيفه
                ActionType: actionType,
                PenaltyType: violationType,
                DeductionDays: deductionDays,
                ExecutionMonth: executionDate,
                DecisionReference: decisionReference,
                Notes: notes ?? string.Empty,
                AttachmentPath: filePath
            );

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = result.Error.Name;
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "تم تعديل الجزاء بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}