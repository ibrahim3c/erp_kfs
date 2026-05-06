using HR.Application.Employees.GetAllEmployees;
using HR.Application.Permissions.CreateLateEntry;
using HR.Application.Permissions.CreatePermission;
using HR.Application.Permissions.DeletePermission;
using HR.Application.Permissions.GetAttendanceLog;
using HR.Domain.Permissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class PermissionsController : Controller
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(int? month, int? year)
        {
            var m = month ?? DateTime.Now.Month;
            var y = year ?? DateTime.Now.Year;

            var query = new GetAttendanceLogQuery(m, y);
            var result = await _mediator.Send(query);

            ViewBag.Month = m;
            ViewBag.Year = y;

            var employees = await _mediator.Send(new GetAllEmployeesQuery());
            ViewBag.Employees = employees.Value ?? new List<EmployeeListResponse>();

            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePermission(
            Guid employeeId,
            string permissionType,
            DateTime date,
            TimeSpan fromTime,
            TimeSpan toTime,
            string? notes)
        {
            var type = Enum.Parse<PermissionType>(permissionType);
            var command = new CreatePermissionCommand(
                employeeId, type, date, fromTime, toTime, notes);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسجيل الإذن بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLateEntry(
            Guid employeeId,
            DateTime date,
            TimeSpan actualArrivalTime,
            string? notes)
        {
            var command = new CreateLateEntryCommand(
                employeeId, date, actualArrivalTime, notes);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسجيل التأخير بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePermission(Guid id)
        {
            var command = new DeletePermissionCommand(id);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم حذف الإذن بنجاح.";

            return RedirectToAction(nameof(Index));
        }
    }
}