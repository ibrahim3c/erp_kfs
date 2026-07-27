using HR.Application.Secondments.Query.GetEmployeesForSelect;
using HR.Application.ServiceTerms.Command.DeleteServiceTerm;
using HR.Application.Terminations.Command.CancelTermination;
using HR.Application.Terminations.Command.CreateTermination;
using HR.Application.Terminations.Command.DeleteTermination;
using HR.Application.Terminations.Query.Details;
using HR.Application.Terminations.Query.List;
using HR.Domain.Terminations.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class TerminationsController : Controller
    {
        private readonly IMediator _mediator;
        public TerminationsController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetTerminationsQuery());

            var employees = await _mediator.Send(new GetEmployeesForSelectQuery(null));
            ViewBag.Employees = employees.IsSuccess ? employees.Value : new();

            return View(result.IsSuccess ? result.Value : new TerminationsResult(new(), 0, 0, 0, 0));
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _mediator.Send(new GetTerminationDetailsQuery(id));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error.Name;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDecision(
            Guid employeeId, string decisionNumber, TerminationReason reason,
            DateTime decisionDate, DateTime lastWorkingDay, string? legalBasis,
            IFormFile? attachmentFile)
        {
            

            var result = await _mediator.Send(new CreateTerminationCommand(
                employeeId, decisionNumber, reason, decisionDate, lastWorkingDay,
                legalBasis, attachmentFile));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم تنفيذ قرار إنهاء الخدمة بنجاح." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid terminationId, string cancellationReason)
        {
            var result = await _mediator.Send(new CancelTerminationCommand(terminationId, cancellationReason));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم إلغاء قرار الإنهاء وإعادة تفعيل الموظف." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid terminationId)
        {
            var result = await _mediator.Send(new DeleteTerminationCommand(terminationId));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم حذف السجل بنجاح." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }
    }
}