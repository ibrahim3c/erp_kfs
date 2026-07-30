using HR.Domain.Employees;
using Modules.Shared.Domain;

namespace HR.Domain.Evaluations
{
    public class Grievance : Entity
    {
        public Guid EmployeeId { get; private set; }
        public GrievanceType GrievanceType { get; private set; }
        public string ComplainedDecisionNumber { get; private set; } = string.Empty;
        public DateTime ComplainedDecisionDate { get; private set; }
        public DateTime SubmissionDate { get; private set; }
        public string Reasons { get; private set; } = string.Empty;
        public string? AttachmentPath { get; private set; }
        public GrievanceStatus Status { get; private set; }
        public string? CommitteeNotes { get; private set; }
        public DateTime? ResolutionDate { get; private set; }

        public Employee Employee { get; private set; } = null!;

        private Grievance() { }

        private Grievance(
            Guid id, Guid employeeId, GrievanceType grievanceType,
            string complainedDecisionNumber, DateTime complainedDecisionDate,
            DateTime submissionDate, string reasons, string? attachmentPath) : base(id)
        {
            EmployeeId = employeeId;
            GrievanceType = grievanceType;
            ComplainedDecisionNumber = complainedDecisionNumber;
            ComplainedDecisionDate = complainedDecisionDate;
            SubmissionDate = submissionDate;
            Reasons = reasons;
            AttachmentPath = attachmentPath;
            Status = GrievanceStatus.Pending;
        }

        public static Result<Grievance> Create(
            Guid employeeId, GrievanceType grievanceType,
            string complainedDecisionNumber, DateTime complainedDecisionDate,
            DateTime submissionDate, string reasons, string? attachmentPath = null)
        {
            if (employeeId == Guid.Empty)
                return Result<Grievance>.Failure(GrievanceErrors.EmployeeRequired);

            if (string.IsNullOrWhiteSpace(complainedDecisionNumber))
                return Result<Grievance>.Failure(GrievanceErrors.DecisionNumberRequired);

            if (string.IsNullOrWhiteSpace(reasons))
                return Result<Grievance>.Failure(GrievanceErrors.ReasonsRequired);

            var grievance = new Grievance(
                Guid.NewGuid(), employeeId, grievanceType,
                complainedDecisionNumber, complainedDecisionDate,
                submissionDate, reasons, attachmentPath);

            return Result<Grievance>.Success(grievance);
        }

        public Result Review()
        {
            if (Status != GrievanceStatus.Pending)
                return Result.Failure(GrievanceErrors.AlreadyResolved);

            Status = GrievanceStatus.UnderReview;
            return Result.Success();
        }

        public Result Resolve(GrievanceStatus newStatus, string? notes, DateTime resolutionDate)
        {
            if (Status == GrievanceStatus.Accepted || Status == GrievanceStatus.Rejected || Status == GrievanceStatus.PartiallyAccepted)
                return Result.Failure(GrievanceErrors.AlreadyResolved);

            if (resolutionDate < SubmissionDate)
                return Result.Failure(GrievanceErrors.InvalidResolutionDate);

            Status = newStatus;
            CommitteeNotes = notes;
            ResolutionDate = resolutionDate;
            return Result.Success();
        }
    }
}
