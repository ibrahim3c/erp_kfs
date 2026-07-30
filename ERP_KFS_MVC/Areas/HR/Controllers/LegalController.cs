using HR.Application.Employees.GetAllEmployees;
using HR.Application.Legal.CreateRuling;
using HR.Application.Legal.ExecuteRuling;
using HR.Application.Legal.GetRulingList;
using HR.Application.Legal.GetRulingStats;
using HR.Application.Legal.UpdateRulingAttachment;
using HR.Domain.Legal;
using ERP_KFS_MVC.Areas.HR.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class LegalController : Controller
    {
        private readonly IMediator _mediator;

        public LegalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Rulings()
        {
            var statsResult = await _mediator.Send(new GetRulingStatsQuery());
            var rulingsResult = await _mediator.Send(new GetRulingListQuery());
            var employeesResult = await _mediator.Send(new GetAllEmployeesQuery());

            var model = new RulingsPageViewModel
            {
                Stats = statsResult.IsSuccess ? statsResult.Value : new GetRulingStatsResponse(),
                Rulings = rulingsResult.IsSuccess ? rulingsResult.Value : new List<GetRulingListResponse>(),
                Employees = employeesResult.IsSuccess ? employeesResult.Value : Enumerable.Empty<EmployeeListResponse>()
            };

            ViewBag.Employees = model.Employees;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRuling(
            string caseNumber,
            string year,
            Guid employeeId,
            string employeeName,
            string summary,
            string executionType,
            IFormFile? attachment)
        {
            string? filePath = null;
            if (attachment is { Length: > 0 })
            {
                var folder = Path.Combine("wwwroot", "uploads", "rulings");
                Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
                var fullPath = Path.Combine(folder, fileName);
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await attachment.CopyToAsync(stream);
                filePath = Path.Combine("uploads", "rulings", fileName);
            }

            if (!Enum.TryParse<RulingExecutionType>(executionType, out var parsedType))
            {
                TempData["ErrorMessage"] = "نوع التنفيذ غير صحيح.";
                return RedirectToAction(nameof(Rulings));
            }

            var command = new CreateRulingCommand(
                caseNumber, year, employeeId, employeeName,
                summary, parsedType, filePath);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسجيل الحكم القضائي بنجاح.";

            return RedirectToAction(nameof(Rulings));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExecuteRuling(Guid rulingId, Guid decisionId)
        {
            var command = new ExecuteRulingCommand(rulingId, decisionId);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تنفيذ الحكم القضائي بنجاح.";

            return RedirectToAction(nameof(Rulings));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAttachment(Guid rulingId, IFormFile? attachment)
        {
            if (attachment is { Length: > 0 })
            {
                var folder = Path.Combine("wwwroot", "uploads", "rulings");
                Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
                var fullPath = Path.Combine(folder, fileName);
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await attachment.CopyToAsync(stream);
                var filePath = Path.Combine("uploads", "rulings", fileName);

                var command = new UpdateRulingAttachmentCommand(rulingId, filePath);
                var result = await _mediator.Send(command);

                TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                    result.IsFailure ? result.Error.Name : "تم رفع المرفق بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "يرجى اختيار ملف لرفعه";
            }

            return RedirectToAction(nameof(Rulings));
        }
    }
}
