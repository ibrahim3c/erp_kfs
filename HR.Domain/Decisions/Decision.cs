using Modules.Shared.Domain;

namespace HR.Domain.Decisions
{
    public sealed class Decision : Entity
    {
        private readonly List<EmployeeDecision> _employeeDecisions = new();

        private Decision() { }

        private Decision(
            Guid id,
            string number,
            Guid decisionTypeId,
            Guid decisionAuthorityId,
            DateTime decisionDate,
            DateTime? validFrom,
            DateTime? validTo,
            bool affectsEmployee,
            bool affectsGroup,
            bool isTemporary,
            string subject,
            string notes,
            string filePath) : base(id)
        {
            Number = number;
            DecisionTypeId = decisionTypeId;
            DecisionAuthorityId = decisionAuthorityId;
            DecisionDate = decisionDate;
            ValidFrom = validFrom;
            ValidTo = validTo;
            AffectsEmployee = affectsEmployee;
            AffectsGroup = affectsGroup;
            IsTemporary = isTemporary;
            Subject = subject;
            Notes = notes;
            FilePath = filePath;

            Status = DecisionStatus.Draft;
        }

        // -------------------------
        // Properties
        // -------------------------

        public string Number { get; private set; }

        public Guid DecisionTypeId { get; private set; }

        public Guid DecisionAuthorityId { get; private set; }

        public DateTime DecisionDate { get; private set; }

        public DateTime? ValidFrom { get; private set; }

        public DateTime? ValidTo { get; private set; }

        public bool AffectsEmployee { get; private set; }

        public bool AffectsGroup { get; private set; }

        public bool IsTemporary { get; private set; }

        public string Subject { get; private set; }

        public string Notes { get; private set; }

        public string FilePath { get; private set; }

        public DecisionStatus Status { get; private set; }

        // Navigation
        public DecisionType DecisionType { get; private set; }

        public DecisionAuthority DecisionAuthority { get; private set; }

        public IReadOnlyCollection<EmployeeDecision> EmployeeDecisions =>
            _employeeDecisions.AsReadOnly();

        // -------------------------
        // Factory
        // -------------------------

        public static Result<Decision> Create(
            string number,
            Guid decisionTypeId,
            Guid decisionAuthorityId,
            DateTime decisionDate,
            DateTime? validFrom,
            DateTime? validTo,
            bool affectsEmployee,
            bool affectsGroup,
            bool isTemporary,
            string subject,
            string notes,
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(number))
                return Result<Decision>.Failure(DecisionErrors.NumberEmpty);

            if (decisionTypeId == Guid.Empty)
                return Result<Decision>.Failure(DecisionErrors.DecisionTypeEmpty);

            if (decisionAuthorityId == Guid.Empty)
                return Result<Decision>.Failure(DecisionErrors.DecisionAuthorityEmpty);

            if (validTo.HasValue && validFrom.HasValue && validTo < validFrom)
                return Result<Decision>.Failure(DecisionErrors.InvalidDates);

            var decision = new Decision(
                Guid.NewGuid(),
                number,
                decisionTypeId,
                decisionAuthorityId,
                decisionDate,
                validFrom,
                validTo,
                affectsEmployee,
                affectsGroup,
                isTemporary,
                subject,
                notes,
                filePath
            );

            return Result<Decision>.Success(decision);
        }

        // -------------------------
        // Business Behaviors
        // -------------------------

        public Result Approve()
        {
            if (Status != DecisionStatus.Draft)
                return Result.Failure(DecisionErrors.AlreadyProcessed);

            Status = DecisionStatus.Approved;
            return Result.Success();
        }

        public Result Reject(string reason)
        {
            if (Status != DecisionStatus.Draft)
                return Result.Failure(DecisionErrors.AlreadyProcessed);

            Status = DecisionStatus.Rejected;
            Notes = reason;

            return Result.Success();
        }

        public Result Archive()
        {
            if (Status == DecisionStatus.Archived)
                return Result.Failure(DecisionErrors.AlreadyArchived);

            Status = DecisionStatus.Archived;
            return Result.Success();
        }

        public void AddEmployeeDecision(EmployeeDecision employeeDecision)
        {
            _employeeDecisions.Add(employeeDecision);
        }
    }
}
