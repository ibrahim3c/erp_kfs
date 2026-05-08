using HR.Domain.Employees;
using HR.Domain.Permissions;
using Modules.Shared.Domain;

namespace HR.Domain.Attendance
{
    public class AttendanceRecord : Entity
    {
        private static readonly TimeSpan OfficialStartTime = new(8, 0, 0);
        private static readonly TimeSpan OfficialEndTime = new(16, 0, 0);

        private AttendanceRecord() { }

        private AttendanceRecord(
            Guid id,
            Guid employeeId,
            DateTime date,
            TimeSpan? checkIn,
            TimeSpan? checkOut,
            AttendanceStatus status,
            string? notes,
            Guid? lateEntryId,
            Guid? permissionRequestId) : base(id)
        {
            EmployeeId = employeeId;
            Date = date;
            CheckIn = checkIn;
            CheckOut = checkOut;
            Status = status;
            Notes = notes;
            LateEntryId = lateEntryId;
            PermissionRequestId = permissionRequestId;
            CalculateDerivedValues();
        }

        public Guid EmployeeId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan? CheckIn { get; private set; }
        public TimeSpan? CheckOut { get; private set; }
        public AttendanceStatus Status { get; private set; }
        public double WorkedHours { get; private set; }
        public int LateMinutes { get; private set; }
        public string? Notes { get; private set; }

        public Guid? LateEntryId { get; private set; }
        public Guid? PermissionRequestId { get; private set; }

        // Navigation
        public Employee Employee { get; private set; }
        public LateEntry? LateEntry { get; private set; }
        public PermissionRequest? PermissionRequest { get; private set; }

        public static Result<AttendanceRecord> Create(
            Guid employeeId,
            DateTime date,
            TimeSpan? checkIn,
            TimeSpan? checkOut,
            AttendanceStatus status,
            string? notes,
            Guid? lateEntryId = null,
            Guid? permissionRequestId = null)
        {
            if (employeeId == Guid.Empty)
                return Result<AttendanceRecord>.Failure(AttendanceErrors.EmployeeRequired);

            if (date == default)
                return Result<AttendanceRecord>.Failure(AttendanceErrors.DateRequired);

            if (date.Date > DateTime.UtcNow.Date)
                return Result<AttendanceRecord>.Failure(AttendanceErrors.FutureDateNotAllowed);

            if (checkIn.HasValue && checkOut.HasValue && checkOut <= checkIn)
                return Result<AttendanceRecord>.Failure(AttendanceErrors.CheckOutBeforeCheckIn);

            var record = new AttendanceRecord(
                Guid.NewGuid(),
                employeeId,
                date,
                checkIn,
                checkOut,
                status,
                notes,
                lateEntryId,
                permissionRequestId);

            return Result<AttendanceRecord>.Success(record);
        }

        public Result RecordCheckIn(TimeSpan time)
        {
            //if (CheckIn.HasValue)
            //    return Result.Failure(AttendanceErrors.DuplicateRecord);

            CheckIn = time;
            CalculateDerivedValues();
            UpdateStatusFromTimes();
            return Result.Success();
        }

        public Result RecordCheckOut(TimeSpan time)
        {
            if (CheckOut.HasValue)
                return Result.Failure(AttendanceErrors.AlreadyCheckedOut);

            if (!CheckIn.HasValue)
                return Result.Failure(AttendanceErrors.MissingCheckIn);

            if (time <= CheckIn.Value)
                return Result.Failure(AttendanceErrors.CheckOutBeforeCheckIn);

            CheckOut = time;
            CalculateDerivedValues();
            return Result.Success();
        }

        public void MarkAbsent()
        {
            Status = AttendanceStatus.Absent;
            CheckIn = null;
            CheckOut = null;
            WorkedHours = 0;
            LateMinutes = 0;
        }

        public void MarkOnMission()
        {
            Status = AttendanceStatus.OnMission;
        }

        public void MarkVacation()
        {
            Status = AttendanceStatus.Vacation;
        }

        public void LinkLateEntry(Guid lateEntryId)
        {
            LateEntryId = lateEntryId;
            Status = AttendanceStatus.Late;
        }

        public void LinkPermission(Guid permissionRequestId)
        {
            PermissionRequestId = permissionRequestId;
            Status = AttendanceStatus.Permission;
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes;
        }

        private void CalculateDerivedValues()
        {
            if (CheckIn.HasValue)
            {
                if (CheckIn.Value > OfficialStartTime)
                    LateMinutes = (int)(CheckIn.Value - OfficialStartTime).TotalMinutes;
                else
                    LateMinutes = 0;

                if (CheckOut.HasValue)
                    WorkedHours = Math.Round((CheckOut.Value - CheckIn.Value).TotalHours, 2);// 2h 30m => 2.5h
                else
                    WorkedHours = 0;
            }
            else
            {
                LateMinutes = 0;
                WorkedHours = 0;
            }
        }

        private void UpdateStatusFromTimes()
        {
            if (CheckIn.HasValue && CheckIn.Value > OfficialStartTime && Status == AttendanceStatus.Present)
                Status = AttendanceStatus.Late;
        }
    }
}
