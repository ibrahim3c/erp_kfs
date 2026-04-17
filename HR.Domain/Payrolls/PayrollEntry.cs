using Modules.Shared.Domain;

namespace HR.Domain.Payrolls
{
    /// <summary>
    /// راتب موظف واحد داخل دورة شهرية
    /// </summary>
    public class PayrollEntry : Entity
    {
        private readonly List<PayrollAdjustment> _adjustments = new();

        private PayrollEntry() { }

        private PayrollEntry(
            Guid id,
            Guid cycleId,
            Guid employeeId,
            decimal basicSalary,
            decimal incentives,
            decimal allowances,
            decimal insuranceDeduction,
            decimal taxDeduction,
            decimal loanDeduction,
            decimal insurancePurchaseDeduction,
            decimal penaltyDeduction) : base(id)
        {
            CycleId = cycleId;
            EmployeeId = employeeId;
            BasicSalary = basicSalary;
            Incentives = incentives;
            Allowances = allowances;
            InsuranceDeduction = insuranceDeduction;
            TaxDeduction = taxDeduction;
            LoanDeduction = loanDeduction;
            InsurancePurchaseDeduction = insurancePurchaseDeduction;
            PenaltyDeduction = penaltyDeduction;
        }

        public Guid CycleId { get; private set; }
        public Guid EmployeeId { get; private set; }

        // ─── الاستحقاقات ───────────────────────────────────────
        /// <summary>الأجر الوظيفي</summary>
        public decimal BasicSalary { get; private set; }

        /// <summary>الحوافز (مكمل)</summary>
        public decimal Incentives { get; private set; }

        /// <summary>بدلات / مكافآت</summary>
        public decimal Allowances { get; private set; }

        // ─── الاستقطاعات الأساسية ──────────────────────────────
        /// <summary>اشتراك التأمينات الاجتماعية</summary>
        public decimal InsuranceDeduction { get; private set; }

        /// <summary>ضريبة الدخل</summary>
        public decimal TaxDeduction { get; private set; }

        /// <summary>قسط السلفة الشهري</summary>
        public decimal LoanDeduction { get; private set; }

        /// <summary>قسط شراء المدة التأمينية</summary>
        public decimal InsurancePurchaseDeduction { get; private set; }

        /// <summary>خصم الجزاءات</summary>
        public decimal PenaltyDeduction { get; private set; }

        // ─── Computed ──────────────────────────────────────────
        public decimal GrossSalary =>
            BasicSalary + Incentives + Allowances;

        public decimal TotalDeductions =>
            InsuranceDeduction
            + TaxDeduction
            + LoanDeduction
            + InsurancePurchaseDeduction
            + PenaltyDeduction
            + _adjustments.Where(a => a.Type == AdjustmentType.Deduction).Sum(a => a.Amount);

        public decimal TotalAdditions =>
            _adjustments.Where(a => a.Type == AdjustmentType.Addition).Sum(a => a.Amount);

        public decimal NetSalary =>
            GrossSalary + TotalAdditions - TotalDeductions;

        public IReadOnlyCollection<PayrollAdjustment> Adjustments =>
            _adjustments.AsReadOnly();

        // ─── Factory ───────────────────────────────────────────
        public static Result<PayrollEntry> Create(
            Guid cycleId,
            Guid employeeId,
            decimal basicSalary,
            decimal incentives,
            decimal allowances,
            decimal insuranceDeduction,
            decimal taxDeduction,
            decimal loanDeduction,
            decimal insurancePurchaseDeduction,
            decimal penaltyDeduction)
        {
            return Result<PayrollEntry>.Success(new PayrollEntry(
                Guid.NewGuid(), cycleId, employeeId,
                basicSalary, incentives, allowances,
                insuranceDeduction, taxDeduction,
                loanDeduction, insurancePurchaseDeduction, penaltyDeduction));
        }

        // ─── Behaviors ─────────────────────────────────────────
        /// <summary>تسوية يدوية — إضافة أو خصم</summary>
        public Result AddAdjustment(AdjustmentType type, decimal amount, string reason)
        {
            if (amount <= 0)
                return Result.Failure(PayrollErrors.InvalidAdjustmentAmount);

            if (string.IsNullOrWhiteSpace(reason))
                return Result.Failure(PayrollErrors.AdjustmentReasonRequired);

            var adjustmentResult = PayrollAdjustment.Create(Id, type, amount, reason);
            if (!adjustmentResult.IsSuccess)
                return Result.Failure(adjustmentResult.Error);

            _adjustments.Add(adjustmentResult.Value);
            return Result.Success();
        }
    }
}