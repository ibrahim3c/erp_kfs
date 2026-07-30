using HR.Domain.Employees;
using Modules.Shared.Domain;

namespace HR.Domain.Leaves
{
    public class LeaveBalance : Entity
    {
        public Guid EmployeeId { get; private set; }
        public int Year { get; private set; }
        public int RegularLeaveEntitled { get; private set; }
        public int RegularLeaveUsed { get; private set; }
        public int CasualLeaveEntitled { get; private set; }
        public int CasualLeaveUsed { get; private set; }
        public int CarryOverRegularDays { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public Employee Employee { get; private set; } = null!;

        public int RegularRemaining => RegularLeaveEntitled + CarryOverRegularDays - RegularLeaveUsed;
        public int CasualRemaining => CasualLeaveEntitled - CasualLeaveUsed;

        private LeaveBalance() { }

        private LeaveBalance(Guid id, Guid employeeId, int year,
            int regularEntitled, int casualEntitled, int carryOver) : base(id)
        {
            EmployeeId = employeeId;
            Year = year;
            RegularLeaveEntitled = regularEntitled;
            RegularLeaveUsed = 0;
            CasualLeaveEntitled = casualEntitled;
            CasualLeaveUsed = 0;
            CarryOverRegularDays = carryOver;
            CreatedAt = DateTime.UtcNow;
        }

        public static LeaveBalance CreateDefault(Guid employeeId, int year, int carryOver = 0)
        {
            return new LeaveBalance(
                Guid.NewGuid(), employeeId, year,
                regularEntitled: 21,
                casualEntitled: 7,
                carryOver: carryOver);
        }

        public Result ConsumeRegular(int days)
        {
            if (days <= 0)
                return Result.Failure(new Error("Leave.InvalidDays", "عدد الأيام يجب أن يكون أكبر من صفر"));

            if (RegularRemaining < days)
                return Result.Failure(LeaveErrors.InsufficientBalance);

            RegularLeaveUsed += days;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result ConsumeCasual(int days)
        {
            if (days <= 0)
                return Result.Failure(new Error("Leave.InvalidDays", "عدد الأيام يجب أن يكون أكبر من صفر"));

            if (CasualRemaining < days)
                return Result.Failure(LeaveErrors.InsufficientBalance);

            CasualLeaveUsed += days;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result ReleaseRegular(int days)
        {
            RegularLeaveUsed = Math.Max(0, RegularLeaveUsed - days);
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result ReleaseCasual(int days)
        {
            CasualLeaveUsed = Math.Max(0, CasualLeaveUsed - days);
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
