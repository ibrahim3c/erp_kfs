using HR.Application.Absence.GetAbsenceSettlementStats;
using HR.Application.Absence.GetUnsettledAbsences;
using HR.Application.Absence.SettleAbsence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class AbsenceController : Controller
    {
        private readonly IMediator _mediator;

        public AbsenceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ══════════════════════════════════════════════════════
        //  صفحة تصفية الغياب
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> Settlement()
        {
            var now = DateTime.Now;

            var statsResult = await _mediator.Send(
                new GetAbsenceSettlementStatsQuery(now.Month, now.Year));
            ViewBag.Stats = statsResult.IsSuccess
                ? statsResult.Value
                : new AbsenceSettlementStatsResponse();

            var absencesResult = await _mediator.Send(
                new GetUnsettledAbsencesQuery(now.Month, now.Year));

            if (absencesResult.IsFailure)
                return View("Error", new { ErrorCode = "Absence.List", ErrorMessage = absencesResult.Error.Name });

            return View(absencesResult.Value);
        }

        // ══════════════════════════════════════════════════════
        //  تنفيذ التسوية
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settle(
            Guid employeeId,
            string actionType,
            string? notes)
        {
            var now = DateTime.Now;

            var command = new SettleAbsenceCommand(
                EmployeeId: employeeId,
                ActionType: actionType,
                Notes: notes,
                Month: now.Month,
                Year: now.Year);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسوية الغياب بنجاح.";

            return RedirectToAction(nameof(Settlement));
        }
    }
}
