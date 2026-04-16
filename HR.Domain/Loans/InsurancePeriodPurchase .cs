using HR.Domain.Employees;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Loans
{
    /// <summary>
    /// شراء مدة خدمة/تأمين — الموظف يشتري سنوات من الهيئة القومية للتأمينات
    /// القسط الشهري بيتخصم من الراتب لحد ما تتسدد التكلفة كاملة
    /// </summary>
    public class InsurancePeriodPurchase : Entity
    {
        private InsurancePeriodPurchase() { }

        private InsurancePeriodPurchase(
            Guid id,
            Guid employeeId,
            string insuranceAuthority,
            int purchasedYears,
            decimal totalCost,
            decimal monthlyInstallment,
            DateTime deductionStartDate,
            string approvalDecisionFilePath) : base(id)
        {
            EmployeeId = employeeId;
            InsuranceAuthority = insuranceAuthority;
            PurchasedYears = purchasedYears;
            TotalCost = totalCost;
            MonthlyInstallment = monthlyInstallment;
            RemainingAmount = totalCost;
            DeductionStartDate = deductionStartDate;
            ApprovalDecisionFilePath = approvalDecisionFilePath;
            Status = InsurancePurchaseStatus.PendingApproval;
            CreatedAt = DateTime.UtcNow;
        }

        // ─── Properties ────────────────────────────────────────────────
        public Guid EmployeeId { get; private set; }    
        public string InsuranceAuthority { get; private set; } //الجهة التأمينية — مثلاً: الهيئة القومية للتأمينات والمعاشات 
      public int PurchasedYears { get; private set; }  //عدد السنوات المشتراة

        public decimal TotalCost { get; private set; }
        public decimal MonthlyInstallment { get; private set; }   //القسط الشهري للخصم من الراتب
        public decimal RemainingAmount { get; private set; }       //المبلغ المتبقي للسداد<
        public DateTime DeductionStartDate { get; private set; }
        public string ApprovalDecisionFilePath { get; private set; }

        public InsurancePurchaseStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        // Navigation
        public Employee Employee { get; private set; }

        // ─── Computed ──────────────────────────────────────────────────
        public bool IsActive => Status == InsurancePurchaseStatus.Approved && RemainingAmount > 0;
        public bool IsCompleted => Status == InsurancePurchaseStatus.Completed;

        // عدد الأشهر المتبقية للسداد تقريبي
        public int EstimatedRemainingMonths =>
            MonthlyInstallment > 0
                ? (int)Math.Ceiling(RemainingAmount / MonthlyInstallment)
                : 0;

        // ─── Factory ───────────────────────────────────────────────────
        public static Result<InsurancePeriodPurchase> Create(
            Guid employeeId,
            string insuranceAuthority,
            int purchasedYears,
            decimal totalCost,
            decimal monthlyInstallment,
            DateTime deductionStartDate,
            string approvalDecisionFilePath)
        {
            if (employeeId == Guid.Empty)
                return Result<InsurancePeriodPurchase>.Failure(InsurancePurchaseErrors.InvalidEmployee);

            if (string.IsNullOrWhiteSpace(insuranceAuthority))
                return Result<InsurancePeriodPurchase>.Failure(InsurancePurchaseErrors.InvalidAuthority);

            if (purchasedYears <= 0)
                return Result<InsurancePeriodPurchase>.Failure(InsurancePurchaseErrors.InvalidYears);

            if (totalCost <= 0)
                return Result<InsurancePeriodPurchase>.Failure(InsurancePurchaseErrors.InvalidTotalCost);

            if (monthlyInstallment <= 0)
                return Result<InsurancePeriodPurchase>.Failure(InsurancePurchaseErrors.InvalidMonthlyInstallment);

            if (monthlyInstallment > totalCost)
                return Result<InsurancePeriodPurchase>.Failure(InsurancePurchaseErrors.InstallmentExceedsTotalCost);

            var purchase = new InsurancePeriodPurchase(
                Guid.NewGuid(),
                employeeId,
                insuranceAuthority,
                purchasedYears,
                totalCost,
                monthlyInstallment,
                deductionStartDate,
                approvalDecisionFilePath);

            return Result<InsurancePeriodPurchase>.Success(purchase);
        }

        // ─── Business Behaviors ────────────────────────────────────────

        /// <summary>اعتماد الطلب — معتمد وسارى</summary>
        public Result Approve()
        {
            if (Status != InsurancePurchaseStatus.PendingApproval)
                return Result.Failure(InsurancePurchaseErrors.AlreadyProcessed);

            Status = InsurancePurchaseStatus.Approved;
            return Result.Success();
        }

        public Result Reject()
        {
            if (Status != InsurancePurchaseStatus.PendingApproval)
                return Result.Failure(InsurancePurchaseErrors.AlreadyProcessed);

            Status = InsurancePurchaseStatus.Rejected;
            return Result.Success();
        }

        /// <summary>
        /// خصم القسط الشهري — بيُستدعى من خدمة الرواتب كل شهر
        /// </summary>
        public Result DeductMonthlyInstallment()
        {
            if (!IsActive)
                return Result.Failure(InsurancePurchaseErrors.NotActive);

            // آخر قسط ممكن يكون أقل من المعتاد
            var amountToDeduct = Math.Min(MonthlyInstallment, RemainingAmount);
            RemainingAmount -= amountToDeduct;

            if (RemainingAmount <= 0)
            {
                RemainingAmount = 0;
                Status = InsurancePurchaseStatus.Completed;
                CompletedAt = DateTime.UtcNow;
            }

            return Result.Success();
        }

        //تحديث القسط الشهري لو اتغير
        public Result UpdateMonthlyInstallment(decimal newInstallment)
        {
            if (!IsActive)
                return Result.Failure(InsurancePurchaseErrors.NotActive);

            if (newInstallment <= 0)
                return Result.Failure(InsurancePurchaseErrors.InvalidMonthlyInstallment);

            MonthlyInstallment = newInstallment;
            return Result.Success();
        }
    }
}

