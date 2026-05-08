using HR.Application.Attendance.Commands.ConvertAbsenceToVacation;
using HR.Application.Attendance.Commands.ConvertLateToPermission;
using HR.Application.Attendance.Commands.CreateManualAttendance;
using HR.Application.Attendance.Commands.ImportAttendanceFromDevice;
using HR.Application.Attendance.Commands.UpdateAttendance;
using HR.Application.Attendance.Queries.GetAbsenceReport;
using HR.Application.Attendance.Queries.GetDailyAttendance;
using HR.Application.Attendance.Queries.GetDailyAttendanceStats;
using HR.Application.Employees.GetAllEmployees;
using HR.Domain.Permissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Organization.Application.IServices;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class AttendanceController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOrganizationService _organizationService;

        public AttendanceController(IMediator mediator, IOrganizationService organizationService)
        {
            _mediator = mediator;
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index(DateTime? date, Guid? orgUnitId, string? status)
        {
            var query = new GetDailyAttendanceQuery(date, orgUnitId, status);
            var result = await _mediator.Send(query);

            await PopulateDropdownsAsync();

            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetStats(DateTime? date)
        {
            var query = new GetDailyAttendanceStatsQuery(date);
            var result = await _mediator.Send(query);

            return Json(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateManual(
            Guid employeeId,
            DateTime date,
            MovementType movementType,
            TimeSpan time,
            string? notes)
        {
            var command = new CreateManualAttendanceCommand(
                employeeId, date, movementType, time, notes);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسجيل الحركة بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            Guid id,
            TimeSpan? checkIn,
            TimeSpan? checkOut,
            string? notes)
        {
            var command = new UpdateAttendanceCommand(id, checkIn, checkOut, notes);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تحديث السجل بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConvertLateToPermission(
            Guid attendanceRecordId,
            int permissionType,
            DateTime date,
            TimeSpan fromTime,
            TimeSpan toTime,
            string? notes)
        {
            var command = new ConvertLateToPermissionCommand(
                attendanceRecordId,
                (PermissionType)permissionType,
                date,
                fromTime,
                toTime,
                notes);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تحويل التأخير إلى إذن بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConvertAbsenceToVacation(
            Guid attendanceRecordId,
            string vacationType,
            string? notes)
        {
            var command = new ConvertAbsenceToVacationCommand(
                attendanceRecordId, vacationType, notes);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تحويل الغياب إلى أجازة بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromDevice(
            List<DeviceRecordDto> records)
        {
            var command = new ImportAttendanceFromDeviceCommand(records);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure
                    ? result.Error.Name
                    : $"تم استيراد {result.Value} سجل من جهاز البصمة بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AbsenceReport(DateTime? dateFrom, DateTime? dateTo, Guid? orgUnitId)
        {
            var from = dateFrom ?? DateTime.Today.AddDays(-30);
            var to = dateTo ?? DateTime.Today;

            var query = new GetAbsenceReportQuery(from, to, orgUnitId);
            var result = await _mediator.Send(query);

            await PopulateDropdownsAsync();

            return View(result.Value);
        }

        private async Task PopulateDropdownsAsync()
        {
            var employees = await _mediator.Send(new GetAllEmployeesQuery());
            ViewBag.Employees = employees.IsSuccess
                ? new SelectList(employees.Value, "Id", "Name")
                : new SelectList(Enumerable.Empty<SelectListItem>());

            var orgUnits = await _organizationService.GetAllOrgUnitsAsync();
            ViewBag.OrgUnitId = orgUnits.IsSuccess
                ? new SelectList(orgUnits.Value.Where(u => u.IsActive), "Id", "Name")
                : new SelectList(Enumerable.Empty<SelectListItem>());
        }
    }
}
