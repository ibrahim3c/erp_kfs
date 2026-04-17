using HR.Domain.Employees;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Payrolls
{
    /// <summary>
    /// دورة رواتب شهر معين — container لكل الإدخالات
    /// </summary>
    public class PayrollCycle : Entity
    {
        private readonly List<PayrollEntry> _entries = new();      

        private PayrollCycle() { }

        private PayrollCycle(Guid id, int month, int year, Guid employmentTypeId) : base(id)
        {
            Month = month;
            Year = year;
            EmploymentTypeId = employmentTypeId;
            Status = PayrollCycleStatus.Draft;
            CreatedAt = DateTime.UtcNow;
        }

        public int Month { get; private set; }
        public int Year { get; private set; }

        /// <summary>فئة الموظفين — الكل / الدائمين / المؤقتين</summary>
        public Guid EmploymentTypeId { get; private set; }
        public EmploymentType EmploymentType { get; private set; }
        public PayrollCycleStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CalculatedAt { get; private set; }
        public DateTime? LockedAt { get; private set; }

        public IReadOnlyCollection<PayrollEntry> Entries => _entries.AsReadOnly();

        // ─── Computed ──────────────────────────────────────────
        public int EmployeeCount => _entries.Count;
        public decimal TotalDeductions => _entries.Sum(e => e.TotalDeductions);
        public decimal TotalNetSalary => _entries.Sum(e => e.NetSalary);

        // ─── Factory ───────────────────────────────────────────
        public static Result<PayrollCycle> Create(int month, int year, Guid employmentTypeId)
        {
            if (month is < 1 or > 12)
                return Result<PayrollCycle>.Failure(PayrollErrors.InvalidMonth);

            if (year < 2000)
                return Result<PayrollCycle>.Failure(PayrollErrors.InvalidYear);

            return Result<PayrollCycle>.Success(
                new PayrollCycle(Guid.NewGuid(), month, year, employmentTypeId));
        }

        // ─── Behaviors ─────────────────────────────────────────
        public void AddEntry(PayrollEntry entry)
        {
            _entries.Add(entry);
        }

        public Result MarkAsCalculated()
        {
            if (Status == PayrollCycleStatus.Locked)
                return Result.Failure(PayrollErrors.CycleLocked);

            Status = PayrollCycleStatus.UnderReview;
            CalculatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Lock()
        {
            if (Status != PayrollCycleStatus.UnderReview)
                return Result.Failure(PayrollErrors.CycleNotReady);

            Status = PayrollCycleStatus.Locked;
            LockedAt = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
