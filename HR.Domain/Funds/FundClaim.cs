using HR.Domain.Employees;
using Modules.Shared.Domain;

namespace HR.Domain.Funds
{
    public class FundClaim : Entity
    {
        public Guid EmployeeId { get; private set; }
        public FundClaimType ClaimType { get; private set; }
        public DateTime EventDate { get; private set; }
        public decimal? Amount { get; private set; }
        public string? AttachmentPath { get; private set; }
        public FundClaimStatus Status { get; private set; }
        public string? CommitteeNotes { get; private set; }
        public string? PaymentOrderNumber { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public Employee Employee { get; private set; } = null!;

        private FundClaim() { }

        private FundClaim(
            Guid id, Guid employeeId, FundClaimType claimType,
            DateTime eventDate, decimal? amount, string? attachmentPath) : base(id)
        {
            EmployeeId = employeeId;
            ClaimType = claimType;
            EventDate = eventDate;
            Amount = amount;
            AttachmentPath = attachmentPath;
            Status = FundClaimStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<FundClaim> Create(
            Guid employeeId, FundClaimType claimType,
            DateTime eventDate, decimal? amount = null, string? attachmentPath = null)
        {
            if (employeeId == Guid.Empty)
                return Result<FundClaim>.Failure(FundErrors.EmployeeRequired);

            if (!Enum.IsDefined(claimType))
                return Result<FundClaim>.Failure(FundErrors.ClaimTypeRequired);

            if (eventDate == default)
                return Result<FundClaim>.Failure(FundErrors.EventDateRequired);

            var claim = new FundClaim(
                Guid.NewGuid(), employeeId, claimType,
                eventDate, amount, attachmentPath);

            return Result<FundClaim>.Success(claim);
        }

        public Result Review(string? notes = null)
        {
            if (Status != FundClaimStatus.Pending)
                return Result.Failure(new Error("Fund.ClaimNotPending", "لا يمكن مراجعة مطالبة ليست قيد المراجعة"));

            Status = FundClaimStatus.UnderReview;
            CommitteeNotes = notes;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Approve(string? notes = null)
        {
            if (Status != FundClaimStatus.Pending && Status != FundClaimStatus.UnderReview)
                return Result.Failure(new Error("Fund.ClaimNotReviewable", "لا يمكن الموافقة على مطالبة بحالة حالية"));

            Status = FundClaimStatus.Approved;
            CommitteeNotes = notes ?? CommitteeNotes;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Reject(string? notes = null)
        {
            if (Status != FundClaimStatus.Pending && Status != FundClaimStatus.UnderReview)
                return Result.Failure(new Error("Fund.ClaimNotReviewable", "لا يمكن رفض مطالبة بحالة حالية"));

            Status = FundClaimStatus.Rejected;
            CommitteeNotes = notes;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result IssuePayment(string paymentOrderNumber)
        {
            if (Status != FundClaimStatus.Approved)
                return Result.Failure(new Error("Fund.ClaimNotApproved", "لا يمكن إصدار أمر صرف لمطالبة غير معتمدة"));

            if (string.IsNullOrWhiteSpace(paymentOrderNumber))
                return Result.Failure(new Error("Fund.PaymentOrderRequired", "رقم أمر الصرف مطلوب"));

            Status = FundClaimStatus.Paid;
            PaymentOrderNumber = paymentOrderNumber;
            PaymentDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
