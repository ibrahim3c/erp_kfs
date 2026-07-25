using HR.Application.Employees.GetAllEmployees;
using HR.Application.Secondments.Command;
using HR.Application.Secondments.Command.EndSecondment;
using HR.Application.Secondments.Command.MarkClearance;
using HR.Application.Secondments.Command.RenewSecondment;
using HR.Application.Secondments.Query.GetActiveSecondments;
using HR.Application.Secondments.Query.GetEmployeesForSelect;
using HR.Application.Secondments.Query.GetSecondmentDetails;
using HR.Domain.Secondments.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class SecondmentsController : Controller
    {
        private readonly IMediator _mediator;
        public SecondmentsController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetActiveSecondmentsQuery());

            var employees = await _mediator.Send(new GetAllEmployeesQuery());
            ViewBag.Employees = employees.Value ?? new List<EmployeeListResponse>();

            return View(result.IsSuccess ? result.Value : new List<SecondmentListItemDto>());
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _mediator.Send(new GetSecondmentDetailsQuery(id));
            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = result.Error.Name;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var employees = await _mediator.Send(new GetEmployeesForSelectQuery(null));
            ViewBag.Employees = employees.IsSuccess ? employees.Value : new List<EmployeeSelectDto>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Guid employeeId, SecondmentType type, string hostEntityName,
            DateTime startDate, DateTime endDate, SalaryBearer salaryBearer, IncentiveBearer incentiveBearer,IFormFile? file)
        {
            var result = await _mediator.Send(new CreateSecondmentCommand(
                employeeId, type, hostEntityName, startDate, endDate, salaryBearer, incentiveBearer, file));

            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = result.Error.Name;
                return RedirectToAction(nameof(Create));
            }

            TempData["SuccessMessage"] = "تم تسجيل الحركة بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renew(Guid secondmentId, DateTime newEndDate)
        {
            var result = await _mediator.Send(new RenewSecondmentCommand(secondmentId, newEndDate));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم تجديد الندب بنجاح." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkClearance(Guid secondmentId)
        {
            var result = await _mediator.Send(new MarkClearanceCommand(secondmentId));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم تسجيل إخلاء الطرف." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> End(Guid secondmentId)
        {
            var result = await _mediator.Send(new EndSecondmentCommand(secondmentId));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم إنهاء الندب/الإعارة." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }
    }
}