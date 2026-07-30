using Modules.Shared.Domain;

namespace HR.Domain.Legal
{
    public sealed class CourtRuling : Entity
    {
        public string CaseNumber { get; private set; }
        public string Year { get; private set; }
        public Guid EmployeeId { get; private set; }
        public string EmployeeName { get; private set; }
        public string Summary { get; private set; }
        public RulingExecutionType ExecutionType { get; private set; }
        public string? AttachmentPath { get; private set; }
        public RulingStatus Status { get; private set; }
        public Guid? DecisionId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ExecutedAt { get; private set; }

        private CourtRuling() { }

        private CourtRuling(
            Guid id,
            string caseNumber,
            string year,
            Guid employeeId,
            string employeeName,
            string summary,
            RulingExecutionType executionType,
            string? attachmentPath) : base(id)
        {
            CaseNumber = caseNumber;
            Year = year;
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            Summary = summary;
            ExecutionType = executionType;
            AttachmentPath = attachmentPath;
            Status = RulingStatus.NotExecuted;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<CourtRuling> Create(
            string caseNumber,
            string year,
            Guid employeeId,
            string employeeName,
            string summary,
            RulingExecutionType executionType,
            string? attachmentPath = null)
        {
            if (string.IsNullOrWhiteSpace(caseNumber))
                return Result<CourtRuling>.Failure(RulingErrors.CaseNumberRequired);

            if (string.IsNullOrWhiteSpace(year))
                return Result<CourtRuling>.Failure(RulingErrors.YearRequired);

            if (employeeId == Guid.Empty)
                return Result<CourtRuling>.Failure(RulingErrors.EmployeeRequired);

            if (string.IsNullOrWhiteSpace(summary))
                return Result<CourtRuling>.Failure(RulingErrors.SummaryRequired);

            if (!Enum.IsDefined(executionType))
                return Result<CourtRuling>.Failure(RulingErrors.ExecutionTypeRequired);

            var ruling = new CourtRuling(
                Guid.NewGuid(),
                caseNumber,
                year,
                employeeId,
                employeeName,
                summary,
                executionType,
                attachmentPath);

            return Result<CourtRuling>.Success(ruling);
        }

        public Result Execute(Guid decisionId)
        {
            if (Status == RulingStatus.Executed)
                return Result.Failure(RulingErrors.AlreadyExecuted);

            DecisionId = decisionId;
            Status = RulingStatus.Executed;
            ExecutedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result UpdateAttachment(string attachmentPath)
        {
            AttachmentPath = attachmentPath;
            return Result.Success();
        }

        public Result MarkInProgress()
        {
            if (Status == RulingStatus.Executed)
                return Result.Failure(RulingErrors.AlreadyExecuted);

            Status = RulingStatus.InProgress;
            return Result.Success();
        }

        public Result Archive()
        {
            if (Status == RulingStatus.NotExecuted)
                return Result.Failure(RulingErrors.NotExecuted);

            return Result.Success();
        }
    }
}
