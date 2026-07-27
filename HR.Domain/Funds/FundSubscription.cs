using HR.Domain.Employees;
using Modules.Shared.Domain;

namespace HR.Domain.Funds
{
    public class FundSubscription : Entity
    {
        public Guid EmployeeId { get; private set; }
        public DateTime SubscriptionDate { get; private set; }
        public FundType FundType { get; private set; }
        public decimal DeductionAmount { get; private set; }
        public bool BankAgreement { get; private set; }
        public FundSubscriptionStatus Status { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public Employee Employee { get; private set; } = null!;

        private FundSubscription() { }

        private FundSubscription(
            Guid id, Guid employeeId, DateTime subscriptionDate,
            FundType fundType, decimal deductionAmount,
            bool bankAgreement, string? notes) : base(id)
        {
            EmployeeId = employeeId;
            SubscriptionDate = subscriptionDate;
            FundType = fundType;
            DeductionAmount = deductionAmount;
            BankAgreement = bankAgreement;
            Notes = notes;
            Status = FundSubscriptionStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<FundSubscription> Create(
            Guid employeeId, DateTime subscriptionDate,
            FundType fundType, decimal deductionAmount,
            bool bankAgreement, string? notes = null)
        {
            if (employeeId == Guid.Empty)
                return Result<FundSubscription>.Failure(FundErrors.EmployeeRequired);

            if (subscriptionDate == default)
                return Result<FundSubscription>.Failure(FundErrors.SubscriptionDateRequired);

            if (!Enum.IsDefined(fundType))
                return Result<FundSubscription>.Failure(FundErrors.FundTypeRequired);

            if (deductionAmount < 0)
                return Result<FundSubscription>.Failure(new Error("Fund.InvalidAmount", "قيمة الخصم لا يمكن أن تكون سالبة"));

            if (!bankAgreement)
                return Result<FundSubscription>.Failure(new Error("Fund.BankAgreementRequired", "يجب موافقة الموظف كتابياً على الخصم من الراتب"));

            var subscription = new FundSubscription(
                Guid.NewGuid(), employeeId, subscriptionDate,
                fundType, deductionAmount, bankAgreement, notes);

            return Result<FundSubscription>.Success(subscription);
        }

        public Result Suspend(string? reason = null)
        {
            if (Status != FundSubscriptionStatus.Active)
                return Result.Failure(new Error("Fund.NotActive", "لا يمكن إيقاف اشتراك غير نشط"));

            Status = FundSubscriptionStatus.Suspended;
            Notes = reason ?? Notes;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Withdraw(string? reason = null)
        {
            if (Status != FundSubscriptionStatus.Active)
                return Result.Failure(new Error("Fund.NotActive", "لا يمكن سحب اشتراك غير نشط"));

            Status = FundSubscriptionStatus.Withdrawn;
            Notes = reason ?? Notes;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Reactivate()
        {
            if (Status == FundSubscriptionStatus.Active)
                return Result.Failure(new Error("Fund.AlreadyActive", "الاشتراك نشط بالفعل"));

            Status = FundSubscriptionStatus.Active;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
