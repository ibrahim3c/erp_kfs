using HR.Application.Retriement.Command.AdvanceStage;
using HR.Application.Retriement.Command.CreateRetirementFile;
using HR.Application.Retriement.Command.UpdateChecklist;
using HR.Application.Retriement.Command.UpdateFinancialData;
using HR.Application.Retriement.Query.GetPendingRetirement;
using HR.Application.Retriement.Query.GetRetirementFile;
using HR.Application.Retriement.Query.GetRetirementFileDetails;
using HR.Domain.Retirement.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Area("HR")]
public class RetirementController : Controller
{
    private readonly IMediator _mediator;
    public RetirementController(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> Pending(int year = 2024)
    {
        var result = await _mediator.Send(new GetPendingRetirementsQuery(year));
        return View(result.IsSuccess ? result.Value : new List<PendingRetirementDto>());
    }

    public async Task<IActionResult> Files()
    {
        var result = await _mediator.Send(new GetRetirementFilesQuery());
        return View(result.IsSuccess ? result.Value : new RetirementFilesResult(new(), 0, 0, 0, 0));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFile(Guid employeeId, DateTime referralDate, RetirementReason reason = RetirementReason.LegalAge)
    {
        var result = await _mediator.Send(new CreateRetirementFileCommand(employeeId, referralDate, reason, null));

        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.Error.Name;
            return RedirectToAction(nameof(Pending));
        }

        TempData["SuccessMessage"] = "تم إنشاء ملف المعاش بنجاح.";
        return RedirectToAction(nameof(Files));
    }

    // ------ بقى صفحة كاملة بدل Modal/JSON ------
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _mediator.Send(new GetRetirementFileDetailsQuery(id));
        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.Error.Name;
            return RedirectToAction(nameof(Files));
        }
        return View(result.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateChecklist(Guid retirementFileId, bool joinPeriodsAdded, bool specialLeavesReviewed)
    {
        var result = await _mediator.Send(new UpdateChecklistCommand(retirementFileId, joinPeriodsAdded, specialLeavesReviewed));

        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess ? "تم حفظ بيانات المراجعة." : result.Error.Name;

        return RedirectToAction(nameof(Details), new { id = retirementFileId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateFinancialData(Guid retirementFileId, int[] years, decimal[] amounts)
    {
        var yearAmounts = years.Zip(amounts, (y, a) => (y, a))
                                .ToDictionary(x => x.y, x => x.a);

        var result = await _mediator.Send(new UpdateFinancialDataCommand(retirementFileId, yearAmounts));

        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess ? "تم حفظ البيانات المالية." : result.Error.Name;

        return RedirectToAction(nameof(Details), new { id = retirementFileId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseFile(Guid retirementFileId)
    {
        var result = await _mediator.Send(new AdvanceStageCommand(retirementFileId, RetirementStage.DeliveredToAuthority));

        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.Error.Name;
            return RedirectToAction(nameof(Details), new { id = retirementFileId });
        }

        TempData["SuccessMessage"] = "تم تحويل حالة الملف إلى \"تم التسليم للأرشيف\".";
        return RedirectToAction(nameof(Files));
    }
}