using HR.Application.Decisions.CreateDecision;
using HR.Application.Decisions.GetDecisionAuthorities;
using HR.Application.Decisions.GetDecisionList;
using HR.Application.Decisions.GetDecisionStats;
using HR.Application.Decisions.GetDecisionTypes;
using HR.Application.Employees.GetAllEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class DecisionsController : Controller
    {
        private readonly IMediator _mediator;

        public DecisionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ══════════════════════════════════════════════════════
        //  الصفحة الرئيسية - سجل القرارات
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var decisionsResult = await _mediator.Send(new GetDecisionListQuery());
            if (decisionsResult.IsFailure)
                return View("Error", new { ErrorCode = "Decisions.List", ErrorMessage = decisionsResult.Error.Name });

            var statsResult = await _mediator.Send(new GetDecisionStatsQuery());
            ViewBag.Stats = statsResult.IsSuccess ? statsResult.Value : new GetDecisionStatsResponse();

            var typesResult = await _mediator.Send(new GetDecisionTypesQuery());
            ViewBag.DecisionTypes = typesResult.IsSuccess ? typesResult.Value : new List<GetDecisionTypeResponse>();

            var authoritiesResult = await _mediator.Send(new GetDecisionAuthoritiesQuery());
            ViewBag.DecisionAuthorities = authoritiesResult.IsSuccess ? authoritiesResult.Value : new List<GetDecisionAuthorityResponse>();

            var employeesResult = await _mediator.Send(new GetAllEmployeesQuery());
            ViewBag.Employees = employeesResult.IsSuccess ? employeesResult.Value : new List<EmployeeListResponse>();

            return View(decisionsResult.Value);
        }

        // ══════════════════════════════════════════════════════
        //  تسجيل قرار جديد
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string number,
            DateTime decisionDate,
            DateTime? validFrom,
            DateTime? validTo,
            Guid decisionTypeId,
            Guid decisionAuthorityId,
            string? subject,
            string? notes,
            IFormFile? scanFile,
            Guid[]? employeeIds)
        {
            string? filePath = null;
            if (scanFile is { Length: > 0 })
            {
                var folder = Path.Combine("wwwroot", "uploads", "decisions");
                Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(scanFile.FileName)}";
                var fullPath = Path.Combine(folder, fileName);
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await scanFile.CopyToAsync(stream);
                filePath = Path.Combine("uploads", "decisions", fileName);
            }

            var command = new CreateDecisionCommand(
                Number: number,
                DecisionDate: decisionDate,
                ValidFrom: validFrom,
                ValidTo: validTo,
                DecisionTypeId: decisionTypeId,
                DecisionAuthorityId: decisionAuthorityId,
                Subject: subject,
                Notes: notes,
                FilePath: filePath,
                AffectsEmployee: true,
                AffectsGroup: false,
                IsTemporary: false,
                EmployeeIds: employeeIds ?? Array.Empty<Guid>()
            );

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسجيل القرار بنجاح.";

            return RedirectToAction(nameof(Index));
        }
    }
}
