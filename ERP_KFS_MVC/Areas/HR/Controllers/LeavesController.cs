using HR.Application.Employees.GetAllEmployees;
using HR.Application.Leaves.ApproveLeaveRequest;
using HR.Application.Leaves.CreateLeaveRequest;
using HR.Application.Leaves.GetLeaveBalance;
using HR.Application.Leaves.GetMedicalLeaveRequests;
using HR.Application.Leaves.GetRegularLeaveRequests;
using HR.Application.Leaves.GetSpecialLeaveRequests;
using HR.Application.Leaves.RejectLeaveRequest;
using HR.Domain.Leaves;
using ERP_KFS_MVC.Areas.HR.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class LeavesController : Controller
    {
        private readonly IMediator _mediator;

        public LeavesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Regular()
        {
            var requestsResult = await _mediator.Send(new GetRegularLeaveRequestsQuery());
            var employeesResult = await _mediator.Send(new GetAllEmployeesQuery());

            var employees = employeesResult.IsSuccess
                ? employeesResult.Value
                : Enumerable.Empty<EmployeeListResponse>();

            var balanceResult = await _mediator.Send(new GetLeaveBalanceQuery(Guid.Empty));

            var model = new LeavePageViewModel
            {
                RegularRequests = requestsResult.IsSuccess ? requestsResult.Value : new(),
                Employees = employees,
                Balance = balanceResult.IsSuccess ? balanceResult.Value : null
            };

            ViewBag.Employees = model.Employees;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRegularRequest(
            Guid employeeId,
            string leaveType,
            DateTime startDate,
            DateTime endDate,
            Guid? replacementEmployeeId,
            string? contactInfo)
        {
            var category = leaveType == "Casual" ? LeaveCategory.Casual : LeaveCategory.Regular;

            var command = new CreateLeaveRequestCommand(
                EmployeeId: employeeId,
                LeaveCategory: category,
                StartDate: startDate,
                EndDate: endDate,
                ReplacementEmployeeId: replacementEmployeeId,
                ContactInfo: contactInfo,
                ReportAuthority: null,
                DecisionNumber: null,
                Diagnosis: null,
                ChildName: null,
                ChildDateOfBirth: null,
                AttachmentPath: null,
                Notes: null,
                PayPercentage: null);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تقديم طلب الأجازة بنجاح.";

            return RedirectToAction(nameof(Regular));
        }

        [HttpGet]
        public async Task<IActionResult> Special()
        {
            var requestsResult = await _mediator.Send(new GetSpecialLeaveRequestsQuery());
            var employeesResult = await _mediator.Send(new GetAllEmployeesQuery());

            var model = new LeavePageViewModel
            {
                SpecialRequests = requestsResult.IsSuccess ? requestsResult.Value : new(),
                Employees = employeesResult.IsSuccess ? employeesResult.Value : Enumerable.Empty<EmployeeListResponse>()
            };

            ViewBag.Employees = model.Employees;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSpecialRequest(
            Guid employeeId,
            string leaveCategory,
            DateTime startDate,
            int duration,
            string? childName,
            DateTime? childDateOfBirth,
            IFormFile? attachment)
        {
            string? filePath = null;
            if (attachment is { Length: > 0 })
            {
                var folder = Path.Combine("wwwroot", "uploads", "leaves");
                Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
                var fullPath = Path.Combine(folder, fileName);
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await attachment.CopyToAsync(stream);
                filePath = Path.Combine("uploads", "leaves", fileName);
            }

            if (!Enum.TryParse<LeaveCategory>(leaveCategory, out var parsedCategory))
            {
                TempData["ErrorMessage"] = "نوع الأجازة غير صحيح.";
                return RedirectToAction(nameof(Special));
            }

            var endDate = startDate.AddDays(duration - 1);

            var command = new CreateLeaveRequestCommand(
                EmployeeId: employeeId,
                LeaveCategory: parsedCategory,
                StartDate: startDate,
                EndDate: endDate,
                ReplacementEmployeeId: null,
                ContactInfo: null,
                ReportAuthority: null,
                DecisionNumber: null,
                Diagnosis: null,
                ChildName: childName,
                ChildDateOfBirth: childDateOfBirth,
                AttachmentPath: filePath,
                Notes: null,
                PayPercentage: null);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تقديم طلب الأجازة الخاصة بنجاح.";

            return RedirectToAction(nameof(Special));
        }

        [HttpGet]
        public async Task<IActionResult> Medical()
        {
            var requestsResult = await _mediator.Send(new GetMedicalLeaveRequestsQuery());
            var employeesResult = await _mediator.Send(new GetAllEmployeesQuery());

            var model = new LeavePageViewModel
            {
                MedicalRequests = requestsResult.IsSuccess ? requestsResult.Value : new(),
                Employees = employeesResult.IsSuccess ? employeesResult.Value : Enumerable.Empty<EmployeeListResponse>()
            };

            ViewBag.Employees = model.Employees;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMedicalRequest(
            Guid employeeId,
            string reportAuthority,
            string? decisionNumber,
            DateTime startDate,
            DateTime endDate,
            int payPercentage,
            string? diagnosis,
            IFormFile? attachment)
        {
            string? filePath = null;
            if (attachment is { Length: > 0 })
            {
                var folder = Path.Combine("wwwroot", "uploads", "leaves", "medical");
                Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
                var fullPath = Path.Combine(folder, fileName);
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await attachment.CopyToAsync(stream);
                filePath = Path.Combine("uploads", "leaves", "medical", fileName);
            }

            var command = new CreateLeaveRequestCommand(
                EmployeeId: employeeId,
                LeaveCategory: LeaveCategory.Medical,
                StartDate: startDate,
                EndDate: endDate,
                ReplacementEmployeeId: null,
                ContactInfo: null,
                ReportAuthority: reportAuthority,
                DecisionNumber: decisionNumber,
                Diagnosis: diagnosis,
                ChildName: null,
                ChildDateOfBirth: null,
                AttachmentPath: filePath,
                Notes: null,
                PayPercentage: payPercentage);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسجيل الأجازة المرضية بنجاح.";

            return RedirectToAction(nameof(Medical));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id)
        {
            var result = await _mediator.Send(new ApproveLeaveRequestCommand(id));

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تماعتماد طلب الأجازة بنجاح.";

            return RedirectToAction(nameof(Regular));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id)
        {
            var result = await _mediator.Send(new RejectLeaveRequestCommand(id));

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم رفض طلب الأجازة.";

            return RedirectToAction(nameof(Regular));
        }
    }
}
