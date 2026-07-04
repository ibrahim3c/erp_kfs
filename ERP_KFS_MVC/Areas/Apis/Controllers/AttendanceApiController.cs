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
    /// <summary>
    ///     API for managing employee attendance records including daily attendance, check-in/out, absence reports,
    ///     and device data import.
    /// </summary>
    [Route("api/attendance")]
    [ApiController]
    public class AttendanceApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendanceApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Retrieves daily attendance records for all employees, optionally filtered by date, organizational unit, or status.
        /// </summary>
        /// <param name="date">Attendance date (defaults to today if omitted).</param>
        /// <param name="orgUnitId">Optional filter by organizational unit ID.</param>
        /// <param name="status">Optional status filter (present, late, absent, mission, vacation, permission).</param>
        /// <returns>List of attendance records with workforce statistics for the given date.</returns>
        /// <response code="200">Returns attendance records and summary statistics.</response>
        /// <response code="400">Invalid query parameters.</response>
        [HttpGet("daily")]
        [ProducesResponseType(typeof(DailyAttendanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Gets aggregated attendance statistics (present, late, absent, mission, vacation counts) for a given date.
        /// </summary>
        /// <param name="date">Date to calculate stats for (defaults to today).</param>
        /// <returns>Attendance summary with counts per status category.</returns>
        /// <response code="200">Returns attendance statistics.</response>
        /// <response code="400">Invalid request.</response>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(DailyAttendanceStatsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStats([FromQuery] DateTime? date)
        {
            var query = new GetDailyAttendanceStatsQuery(date);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(new { error = result.Error.Name });

            return Ok(result.Value);
        }

        /// <summary>
        ///     Gets the attendance record for a specific employee on a given date.
        /// </summary>
        /// <param name="employeeId">The employee's unique identifier.</param>
        /// <param name="date">Optional date filter (defaults to today).</param>
        /// <param name="orgUnitId">Optional organizational unit filter.</param>
        /// <param name="status">Optional status filter.</param>
        /// <returns>Attendance record for the specified employee.</returns>
        /// <response code="200">Returns the employee's attendance record.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="404">No attendance record found for this employee.</response>
        [HttpGet("employee/{employeeId:guid}")]
        [ProducesResponseType(typeof(AttendanceRowDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        ///     Gets the absence history for a specific employee within a date range.
        /// </summary>
        /// <param name="employeeId">The employee's unique identifier.</param>
        /// <param name="dateFrom">Start date (defaults to 30 days ago).</param>
        /// <param name="dateTo">End date (defaults to today).</param>
        /// <returns>Absence report with total absence days and list of absent dates for the employee.</returns>
        /// <response code="200">Returns absence history (or empty report if no absences found).</response>
        /// <response code="400">Invalid request parameters.</response>
        [HttpGet("employee/{employeeId:guid}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Records an employee check-in (manual entry).
        /// </summary>
        /// <param name="request">Check-in details including employee ID, date, and time.</param>
        /// <returns>Confirmation message with the new attendance record ID.</returns>
        /// <response code="200">Check-in recorded successfully.</response>
        /// <response code="400">Invalid request data.</response>
        [HttpPost("check-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Records an employee check-out (manual entry).
        /// </summary>
        /// <param name="request">Check-out details including employee ID, date, and time.</param>
        /// <returns>Confirmation message with the new attendance record ID.</returns>
        /// <response code="200">Check-out recorded successfully.</response>
        /// <response code="400">Invalid request data.</response>
        [HttpPost("check-out")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Creates a manual attendance record with a specified movement type (check-in, check-out, etc.).
        /// </summary>
        /// <param name="request">Manual attendance entry details.</param>
        /// <returns>Confirmation message with the new record ID.</returns>
        /// <response code="200">Attendance record created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        [HttpPost("manual")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Updates an existing attendance record (check-in time, check-out time, notes).
        /// </summary>
        /// <param name="id">The attendance record ID.</param>
        /// <param name="request">Updated attendance data.</param>
        /// <returns>Confirmation message with the updated record ID.</returns>
        /// <response code="200">Attendance record updated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Imports attendance records from a biometric fingerprint device.
        /// </summary>
        /// <param name="records">List of device records to import.</param>
        /// <returns>Number of records successfully imported.</returns>
        /// <response code="200">Import completed successfully.</response>
        /// <response code="400">No records provided or import failed.</response>
        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Converts a late attendance record into an official permission (إذن).
        /// </summary>
        /// <param name="id">The attendance record ID to convert.</param>
        /// <param name="request">Permission details including type, date, and time range.</param>
        /// <returns>Confirmation message.</returns>
        /// <response code="200">Late record converted to permission successfully.</response>
        /// <response code="400">Conversion failed.</response>
        [HttpPost("{id:guid}/convert-to-permission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Converts an absence record into an official vacation (أجازة).
        /// </summary>
        /// <param name="id">The attendance record ID to convert.</param>
        /// <param name="request">Vacation details including type and notes.</param>
        /// <returns>Confirmation message.</returns>
        /// <response code="200">Absence converted to vacation successfully.</response>
        /// <response code="400">Conversion failed.</response>
        [HttpPost("{id:guid}/convert-to-vacation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        ///     Generates an absence report for all employees within a specified date range, optionally filtered by organizational unit.
        /// </summary>
        /// <param name="dateFrom">Report start date (defaults to 30 days ago).</param>
        /// <param name="dateTo">Report end date (defaults to today).</param>
        /// <param name="orgUnitId">Optional organizational unit filter.</param>
        /// <returns>Absence report with employee details, total absence days, and absent date lists.</returns>
        /// <response code="200">Returns absence report data.</response>
        /// <response code="400">Invalid query parameters.</response>
        [HttpGet("absence-report")]
        [ProducesResponseType(typeof(AbsenceReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
