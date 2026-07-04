using HR.Application.Attendance.Commands.ConvertAbsenceToVacation;
using HR.Application.Attendance.Commands.ConvertLateToPermission;
using HR.Application.Attendance.Commands.CreateManualAttendance;
using HR.Application.Attendance.Commands.ImportAttendanceFromDevice;
using HR.Application.Attendance.Commands.UpdateAttendance;
using HR.Application.Attendance.Queries.GetAbsenceReport;
using HR.Application.Attendance.Queries.GetDailyAttendance;
using HR.Application.Attendance.Queries.GetDailyAttendanceStats;
using ERP_KFS_MVC.Areas.Apis.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.Apis.Controllers
{
    [Route("api/attendance")]
    [ApiController]
    public class AttendanceApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendanceApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyAttendance(
            [FromQuery] DateTime? date,
            [FromQuery] Guid? orgUnitId,
            [FromQuery] string? status)
        {
            var query = new GetDailyAttendanceQuery(date, orgUnitId, status);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(result.Value);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] DateTime? date)
        {
            var query = new GetDailyAttendanceStatsQuery(date);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(result.Value);
        }

        [HttpGet("employee/{employeeId:guid}")]
        public async Task<IActionResult> GetByEmployee(
            Guid employeeId,
            [FromQuery] DateTime? date,
            [FromQuery] Guid? orgUnitId,
            [FromQuery] string? status)
        {
            var query = new GetDailyAttendanceQuery(date, orgUnitId, status);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            var employeeRecord = result.Value.Items.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employeeRecord is null)
                return NotFound(new { error = "لم يتم العثور على سجل حضور للموظف" });

            return Ok(employeeRecord);
        }

        [HttpGet("employee/{employeeId:guid}/history")]
        public async Task<IActionResult> GetEmployeeHistory(
            Guid employeeId,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo)
        {
            var from = dateFrom ?? DateTime.Today.AddDays(-30);
            var to = dateTo ?? DateTime.Today;

            var query = new GetAbsenceReportQuery(from, to, null);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            var employeeReport = result.Value.Items.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employeeReport is null)
                return Ok(new
                {
                    employeeId,
                    dateFrom = from,
                    dateTo = to,
                    absenceDays = 0,
                    absentDates = Array.Empty<string>()
                });

            return Ok(new
            {
                employeeId = employeeReport.EmployeeId,
                employeeName = employeeReport.EmployeeName,
                jobTitle = employeeReport.JobTitleName,
                department = employeeReport.DepartmentName,
                dateFrom = result.Value.DateFrom,
                dateTo = result.Value.DateTo,
                absenceDays = employeeReport.AbsenceDays,
                absentDates = employeeReport.AbsentDates
            });
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn(
            [FromBody] CheckInOutRequest request)
        {
            var command = new CreateManualAttendanceCommand(
                request.EmployeeId,
                request.Date,
                MovementType.CheckIn,
                request.Time,
                request.Notes);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(new { id = result.Value, message = "تم تسجيل الحضور بنجاح" });
        }

        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut(
            [FromBody] CheckInOutRequest request)
        {
            var command = new CreateManualAttendanceCommand(
                request.EmployeeId,
                request.Date,
                MovementType.CheckOut,
                request.Time,
                request.Notes);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(new { id = result.Value, message = "تم تسجيل الانصراف بنجاح" });
        }

        [HttpPost("manual")]
        public async Task<IActionResult> CreateManual(
            [FromBody] CreateManualAttendanceRequest request)
        {
            var command = new CreateManualAttendanceCommand(
                request.EmployeeId,
                request.Date,
                request.MovementType,
                request.Time,
                request.Notes);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(new { id = result.Value, message = "تم تسجيل الحركة بنجاح" });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateAttendanceRequest request)
        {
            var command = new UpdateAttendanceCommand(
                id,
                request.CheckIn,
                request.CheckOut,
                request.Notes);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(new { id = result.Value, message = "تم تحديث السجل بنجاح" });
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFromDevice(
            [FromBody] List<DeviceRecordDto> records)
        {
            if (records is null || records.Count == 0)
                return BadRequest(new { error = "لم يتم إرسال أي سجلات للاستيراد" });

            var command = new ImportAttendanceFromDeviceCommand(records);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(new
            {
                importedCount = result.Value,
                message = $"تم استيراد {result.Value} سجل من جهاز البصمة بنجاح"
            });
        }

        [HttpPost("{id:guid}/convert-to-permission")]
        public async Task<IActionResult> ConvertLateToPermission(
            Guid id,
            [FromBody] ConvertToPermissionRequest request)
        {
            var command = new ConvertLateToPermissionCommand(
                id,
                request.PermissionType,
                request.Date,
                request.FromTime,
                request.ToTime,
                request.Notes);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(new { message = "تم تحويل التأخير إلى إذن بنجاح" });
        }

        [HttpPost("{id:guid}/convert-to-vacation")]
        public async Task<IActionResult> ConvertAbsenceToVacation(
            Guid id,
            [FromBody] ConvertToVacationRequest request)
        {
            var command = new ConvertAbsenceToVacationCommand(
                id,
                request.VacationType,
                request.Notes);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(new { message = "تم تحويل الغياب إلى أجازة بنجاح" });
        }

        [HttpGet("absence-report")]
        public async Task<IActionResult> GetAbsenceReport(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] Guid? orgUnitId)
        {
            var from = dateFrom ?? DateTime.Today.AddDays(-30);
            var to = dateTo ?? DateTime.Today;

            var query = new GetAbsenceReportQuery(from, to, orgUnitId);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(result.Value);
        }
    }
}
