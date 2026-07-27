using HR.Application.Employees.GetAllEmployees;
using HR.Application.Secondments.Query.GetEmployeesForSelect;
using HR.Application.ServiceTerms.Command.ApproveServiceTerm;
using HR.Application.ServiceTerms.Command.CreateServiceTerm;
using HR.Application.ServiceTerms.Command.DeleteServiceTerm;
using HR.Application.ServiceTerms.Command.RejectServiceTerm;
using HR.Application.ServiceTerms.Query.GetServiceTermDetails;
using HR.Application.ServiceTerms.Query.GetServiceTerms;
using HR.Domain.ServiceTerms.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{

    [Area("HR")]
    public class ServiceTermController : Controller
    {
        private readonly IMediator _mediator;
        public ServiceTermController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetServiceTermsQuery());

            var employees = await _mediator.Send(new GetAllEmployeesQuery());
            ViewBag.Employees = employees.Value ?? new List<EmployeeListResponse>();

            return View(result.IsSuccess ? result.Value : new List<ServiceTermListItemDto>());
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _mediator.Send(new GetServiceTermDetailsQuery(id));
            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = result.Error.Name;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Guid employeeId, string previousEntityName, ServiceType type,
            DateTime startDate, DateTime endDate, string? committeeDecisionNumber,
            IFormFile? attachmentFile)
        {
            
            var result = await _mediator.Send(new CreateServiceTermCommand(
                employeeId, previousEntityName, type, startDate, endDate,
                committeeDecisionNumber, attachmentFile));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم تسجيل طلب ضم المدة بنجاح." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid serviceTermId)
        {
            var result = await _mediator.Send(new ApproveServiceTermCommand(serviceTermId));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم اعتماد ضم المدة بنجاح." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid serviceTermId, string reason)
        {
            var result = await _mediator.Send(new RejectServiceTermCommand(serviceTermId, reason));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم رفض الطلب." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid serviceTermId)
        {
            var result = await _mediator.Send(new DeleteServiceTermCommand(serviceTermId));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم حذف السجل بنجاح." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }
    }
}