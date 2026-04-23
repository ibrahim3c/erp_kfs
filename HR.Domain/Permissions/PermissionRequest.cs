using HR.Domain.Employees;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Permissions
{
    public class PermissionRequest : Entity
    {
        private PermissionRequest() { }

        private PermissionRequest(
            Guid id,
            Guid employeeId,
            PermissionType permissionType,
            DateTime date,
            TimeSpan fromTime,
            TimeSpan toTime,
            string? notes) : base(id)
        {
            EmployeeId = employeeId;
            PermissionType = permissionType;
            Date = date;
            FromTime = fromTime;
            ToTime = toTime;
            Notes = notes;
            DurationMinutes = (int)(toTime - fromTime).TotalMinutes;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid EmployeeId { get; private set; }
        public PermissionType PermissionType { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan FromTime { get; private set; }
        public TimeSpan ToTime { get; private set; }

        public int DurationMinutes { get; private set; }     // المدة بالدقائق — محسوبة تلقائيا
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navigation
        public Employee Employee { get; private set; }

        // ─── Factory ───────────────────────────────────────────
        public static Result<PermissionRequest> Create(
            Guid employeeId,
            PermissionType permissionType,
            DateTime date,
            TimeSpan fromTime,
            TimeSpan toTime,
            string? notes)
        {
            if (employeeId == Guid.Empty)
                return Result<PermissionRequest>.Failure(AttendanceErrors.EmployeeRequired);

            if (toTime <= fromTime)
                return Result<PermissionRequest>.Failure(AttendanceErrors.InvalidTimeRange);

            var duration = (int)(toTime - fromTime).TotalMinutes;

            // الحد الأقصى للإذن الشخصي 4 ساعات = 240 دقيقة
            if (permissionType == PermissionType.Personal && duration > 240)
                return Result<PermissionRequest>.Failure(AttendanceErrors.PersonalPermissionExceeded);

            return Result<PermissionRequest>.Success(
                new PermissionRequest(
                    Guid.NewGuid(), employeeId, permissionType,
                    date, fromTime, toTime, notes));
        }
    }
}
