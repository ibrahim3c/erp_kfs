using HR.Application.Promotions.Commands;
using HR.Application.Promotions.Queries;
using HR.Domain.Promotions.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    // الصفحة دي هي محرك قرار الترقيات والعلاوات — بتقولك مين من الموظفين وصل لحقه في ترقية أو علاوة، وليه، وإيه اللي بيمنعه.
    // العلاوة == هي زيادة مالية على المرتب بدون تغيير الدرجة الوظيفية.
    [Area("HR")]
    public class PromotionsController : Controller
    {
        private readonly IMediator _mediator;

        public PromotionsController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet]
        public IActionResult Index() => View();

        // ── AJAX: عرض كشف المستحقين ──────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(
       CycleType cycleType,
       string eligibilityDate,
       int minKpiScore,
       decimal? maxPenaltyDays,
       CancellationToken ct)
        {
            var userId = GetCurrentUserId();

            if (!DateTime.TryParse(eligibilityDate, out var parsedDate))
                // ✅ بدل PartialView("_Error") — بنرجع HTML مباشرة
                return Content(
                    "<div class='alert alert-danger m-3'>" +
                    "<i class='fas fa-exclamation-circle me-2'></i>" +
                    "تاريخ الاستحقاق غير صحيح</div>",
                    "text/html");

            var query = new CheckEligibilityQuery(
                cycleType, parsedDate, minKpiScore, maxPenaltyDays, userId);

            var result = await _mediator.Send(query, ct);

            if (result.IsFailure)
                // ✅ نفس الحل — Content بدل PartialView("_Error")
                return Content(
                    $"<div class='alert alert-danger m-3'>" +
                    $"<i class='fas fa-exclamation-circle me-2'></i>" +
                    $"{result.Error.Name}</div>",
                    "text/html");

            return PartialView("_ResultsTable", result.Value);
        }

        // ── AJAX: اعتماد الكشف ───────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(
            Guid cycleId,
            List<Guid> selectedIds,
            CancellationToken ct)
        {
            if (cycleId == Guid.Empty)
                return Json(new { success = false, message = "الكشف غير محدد" });

            if (selectedIds is null || !selectedIds.Any())
                return Json(new { success = false, message = "يجب اختيار موظف واحد على الأقل" });

            var command = new ApprovePromotionCommand(
                cycleId,
                selectedIds,
                GetCurrentUserId());   // ✅ من الـ Claims

            var result = await _mediator.Send(command, ct);

            // ✅ بنرجع JSON متوافق مع الـ JS
            return result.IsSuccess
                ? Json(new
                {
                    success = true,
                    message = result.Value.Message,
                    count = result.Value.ApprovedCount
                })
                : Json(new
                {
                    success = false,
                    message = result.Error.Name
                });
        }

        // ── Helper ────────────────────────────────────────────────
        private Guid GetCurrentUserId()
        {
            var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }
    }
}