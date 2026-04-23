using HR.Domain.Employees;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Permissions
{
    public class LateEntry : Entity
    {
        // وقت الدوام الرسمي الثابت
        private static readonly TimeSpan OfficialStartTime = new(8, 0, 0);

        // الحد الشهري — كل 180 دقيقة (3 ساعات) = يوم جزاء
        public const int MinutesPerPenaltyDay = 180;

        private LateEntry() { }

        private LateEntry(
            Guid id,
            Guid employeeId,
            DateTime date,
            TimeSpan actualArrivalTime,
            string? notes) : base(id)
        {
            EmployeeId = employeeId;
            Date = date;
            ActualArrivalTime = actualArrivalTime;
            LateMinutes = CalculateLateMinutes(actualArrivalTime);
            Notes = notes;
            IsTransferredToPenalty = false;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid EmployeeId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan ActualArrivalTime { get; private set; }

        // دقائق التأخير المحسوبة تلقائياً
        public int LateMinutes { get; private set; }
        public string? Notes { get; private set; }

        // هل تم تحويله لجزاء？
        public bool IsTransferredToPenalty { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navigation
        public Employee Employee { get; private set; }

        // ─── Factory ───────────────────────────────────────────
        public static Result<LateEntry> Create(
            Guid employeeId,
            DateTime date,
            TimeSpan actualArrivalTime,
            string? notes)
        {
            if (employeeId == Guid.Empty)
                return Result<LateEntry>.Failure(AttendanceErrors.EmployeeRequired);

            if (actualArrivalTime <= OfficialStartTime)
                return Result<LateEntry>.Failure(AttendanceErrors.NotLate);

            return Result<LateEntry>.Success(
                new LateEntry(Guid.NewGuid(), employeeId, date, actualArrivalTime, notes));
        }

        // ─── Behaviors ─────────────────────────────────────────
        public void MarkAsTransferredToPenalty()
        {
            IsTransferredToPenalty = true;
        }

        // ─── Private ───────────────────────────────────────────
        private static int CalculateLateMinutes(TimeSpan actualArrivalTime)
            => (int)(actualArrivalTime - OfficialStartTime).TotalMinutes;
    }
}
