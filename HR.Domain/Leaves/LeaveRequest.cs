using HR.Domain.Employees;
using Modules.Shared.Domain;

namespace HR.Domain.Leaves
{
    public class LeaveRequest : Entity
    {
        public Guid EmployeeId { get; private set; }
        public LeaveCategory LeaveCategory { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int DurationDays { get; private set; }
        public LeaveStatus Status { get; private set; }
        public SalaryStatusType SalaryStatus { get; private set; }
        public decimal? PayPercentage { get; private set; }

        // Regular/Casual fields
        public Guid? ReplacementEmployeeId { get; private set; }
        public string? ContactInfo { get; private set; }

        // Medical fields
        public string? ReportAuthority { get; private set; }
        public string? DecisionNumber { get; private set; }
        public string? Diagnosis { get; private set; }

        // Maternity/ChildCare fields
        public string? ChildName { get; private set; }
        public DateTime? ChildDateOfBirth { get; private set; }

        // Attachment
        public string? AttachmentPath { get; private set; }

        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ApprovedAt { get; private set; }

        public Employee Employee { get; private set; } = null!;
        public Employee? ReplacementEmployee { get; private set; }

        private LeaveRequest() { }

        private LeaveRequest(
            Guid id, Guid employeeId, LeaveCategory leaveCategory,
            DateTime startDate, DateTime endDate,
            Guid? replacementEmployeeId, string? contactInfo,
            string? reportAuthority, string? decisionNumber, string? diagnosis,
            string? childName, DateTime? childDateOfBirth,
            string? attachmentPath, string? notes) : base(id)
        {
            EmployeeId = employeeId;
            LeaveCategory = leaveCategory;
            StartDate = startDate;
            EndDate = endDate;
            DurationDays = (endDate - startDate).Days + 1;
            Status = LeaveStatus.Pending;
            SalaryStatus = DetermineSalaryStatus(leaveCategory);
            ReplacementEmployeeId = replacementEmployeeId;
            ContactInfo = contactInfo;
            ReportAuthority = reportAuthority;
            DecisionNumber = decisionNumber;
            Diagnosis = diagnosis;
            ChildName = childName;
            ChildDateOfBirth = childDateOfBirth;
            AttachmentPath = attachmentPath;
            Notes = notes;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<LeaveRequest> Create(
            Guid employeeId, LeaveCategory leaveCategory,
            DateTime startDate, DateTime endDate,
            Guid? replacementEmployeeId = null, string? contactInfo = null,
            string? reportAuthority = null, string? decisionNumber = null, string? diagnosis = null,
            string? childName = null, DateTime? childDateOfBirth = null,
            string? attachmentPath = null, string? notes = null,
            decimal? payPercentage = null)
        {
            if (employeeId == Guid.Empty)
                return Result<LeaveRequest>.Failure(LeaveErrors.EmployeeRequired);

            if (!Enum.IsDefined(leaveCategory))
                return Result<LeaveRequest>.Failure(LeaveErrors.CategoryRequired);

            if (startDate == default)
                return Result<LeaveRequest>.Failure(LeaveErrors.StartDateRequired);

            if (endDate == default)
                return Result<LeaveRequest>.Failure(LeaveErrors.EndDateRequired);

            if (endDate < startDate)
                return Result<LeaveRequest>.Failure(LeaveErrors.InvalidDateRange);

            var request = new LeaveRequest(
                Guid.NewGuid(), employeeId, leaveCategory,
                startDate, endDate,
                replacementEmployeeId, contactInfo,
                reportAuthority, decisionNumber, diagnosis,
                childName, childDateOfBirth,
                attachmentPath, notes);

            if (payPercentage.HasValue)
                request.PayPercentage = payPercentage;

            return Result<LeaveRequest>.Success(request);
        }

        public Result Approve()
        {
            if (Status != LeaveStatus.Pending)
                return Result.Failure(LeaveErrors.NotPending);

            Status = LeaveStatus.Approved;
            ApprovedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Reject()
        {
            if (Status != LeaveStatus.Pending)
                return Result.Failure(LeaveErrors.NotPending);

            Status = LeaveStatus.Rejected;
            return Result.Success();
        }

        public Result Cancel()
        {
            if (Status != LeaveStatus.Approved)
                return Result.Failure(new Error("Leave.CannotCancel", "لا يمكن إلغاء طلب غير معتمد"));

            Status = LeaveStatus.Cancelled;
            return Result.Success();
        }

        private static SalaryStatusType DetermineSalaryStatus(LeaveCategory category)
        {
            return category switch
            {
                LeaveCategory.Regular => SalaryStatusType.FullPay,
                LeaveCategory.Casual => SalaryStatusType.FullPay,
                LeaveCategory.Maternity => SalaryStatusType.FullPay,
                LeaveCategory.Hajj => SalaryStatusType.FullPay,
                LeaveCategory.ChildCare => SalaryStatusType.NoPay,
                LeaveCategory.Exam => SalaryStatusType.FullPay,
                LeaveCategory.Medical => SalaryStatusType.FullPay,
                _ => SalaryStatusType.FullPay
            };
        }
    }
}
