using HR.Domain.Employees;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Loans
{
    public class Loan : Entity
    {
        private Loan() { }

        private Loan(Guid id, Guid employeeId, decimal amount, int months,
            decimal installmentAmount, DateTime startDate, string reason) : base(id)
        {
            EmployeeId = employeeId;
            Amount = amount;
            Months = months;
            InstallmentAmount = installmentAmount;
            StartDate = startDate;
            Reason = reason;
            RemainingAmount = amount;
            IsCompleted = false;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid EmployeeId { get; private set; }
        public decimal Amount { get; private set; }
        public int Months { get; private set; }
        public decimal InstallmentAmount { get; private set; }
        public decimal RemainingAmount { get; private set; }
        public DateTime StartDate { get; private set; }
        public string Reason { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        // Navigation Props
        public Employee Employee { get; private set; }
        public IReadOnlyCollection<LoanInstallment> Installments => _installments.AsReadOnly();
        private readonly List<LoanInstallment> _installments = new();

        // ─── Factory ───────────────────────────────────────────────
        public static Result<Loan> Create(Guid employeeId, decimal amount, int months,
            DateTime startDate, string reason)
        {
            if (amount <= 0) return Result<Loan>.Failure(LoanErrors.InvalidAmount);
            if (months <= 0) return Result<Loan>.Failure(LoanErrors.InvalidMonths);

            decimal installmentAmount = Math.Round(amount / months, 2);

            var loan = new Loan(Guid.NewGuid(), employeeId, amount,
                months, installmentAmount, startDate, reason);

            // توليد الأقساط تلقائياً عند إنشاء السلفة
            loan.GenerateInstallments();

            return Result<Loan>.Success(loan);
        }

        // ─── Pay Installment ────────────────────────────────────────
        /// <summary>
        /// خصم قسط — بيُستدعى من خدمة الرواتب كل شهر
        /// </summary>
        public Result PayNextInstallment(DateTime paymentDate)
        {
            if (IsCompleted)
                return Result.Failure(LoanErrors.LoanAlreadyCompleted);

            var nextInstallment = _installments
                .Where(i => !i.IsPaid)
                .OrderBy(i => i.DueDate)
                .FirstOrDefault();

            if (nextInstallment is null)
                return Result.Failure(LoanErrors.NoRemainingInstallments);

            nextInstallment.MarkAsPaid(paymentDate);

            RemainingAmount -= nextInstallment.Amount;

            // لو باقي مبلغ صغير جداً بسبب تقريب → صفّره
            if (RemainingAmount < 0) RemainingAmount = 0;

            // تحقق إذا خلصت كل الأقساط
            if (_installments.All(i => i.IsPaid))
                MarkAsCompleted();

            return Result.Success();
        }

        // ─── Early Settlement (مخالصة) ──────────────────────────────
        /// <summary>
        /// مخالصة كاملة — الموظف بيسدد الباقي دفعة واحدة
        /// </summary>
        public Result Settle(DateTime settlementDate)
        {
            if (IsCompleted)
                return Result.Failure(LoanErrors.LoanAlreadyCompleted);

            // دفع كل الأقساط غير المدفوعة
            foreach (var installment in _installments.Where(i => !i.IsPaid))
                installment.MarkAsPaid(settlementDate);

            RemainingAmount = 0;
            MarkAsCompleted();

            return Result.Success();
        }

        // ─── Private Helpers ────────────────────────────────────────
        private void GenerateInstallments()
        {
            for (int i = 0; i < Months; i++)
            {
                var dueDate = StartDate.AddMonths(i);
                var installmentAmount = (i == Months - 1)
                    ? Amount - (InstallmentAmount * (Months - 1))  // آخر قسط يأخذ الفرق لضمان الدقة
                    : InstallmentAmount;

                _installments.Add(LoanInstallment.Create(Id, installmentAmount, dueDate).Value!);
            }
        }

        private void MarkAsCompleted()
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }

        // ─── Computed / Query Helpers ────────────────────────────────
        public int PaidInstallmentsCount => _installments.Count(i => i.IsPaid);  //عدد الأقساط المدفوعة
        public int RemainingInstallmentsCount => _installments.Count(i => !i.IsPaid);    //عدد الأقساط المتبقية
        public string StatusLabel => IsCompleted ? "خالصة" : "(سارية (بخصم";         // حالة السلفة للعرض — سارية (بخصم) / خالصة
    }
}
