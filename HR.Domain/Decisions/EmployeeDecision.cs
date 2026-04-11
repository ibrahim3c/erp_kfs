using HR.Domain.Employees;
using Modules.Shared.Domain;
namespace HR.Domain.Decisions
{
    public sealed class EmployeeDecision : Entity
    {
        private EmployeeDecision() { }
        private EmployeeDecision(Guid id,Guid employeeId, Guid decisionId, string description, DateTime? validFrom, DateTime? validTo, EmployeeDecisionStatus status, string notes):base(id)
        {
            EmployeeId = employeeId;
            DecisionId = decisionId;
            Description = description;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Status = status;
            Notes = notes;
        }
        public static Result<EmployeeDecision> Create(
                  Guid employeeId,
                  Guid decisionId,
                  string description,
                  DateTime? validFrom,
                  DateTime? validTo,
                  EmployeeDecisionStatus status,
                  string notes)
        {
            if (employeeId == Guid.Empty)
                return Result<EmployeeDecision>.Failure(DecisionErrors.EmployeeIdEmpty);

            if (decisionId == Guid.Empty)
                return Result<EmployeeDecision>.Failure(DecisionErrors.DecisionIdEmpty);

            if (validTo.HasValue && validFrom.HasValue && validTo < validFrom)
                return Result<EmployeeDecision>.Failure(EmployeeErrors.InvalidDecisionDates);

            var decision = new EmployeeDecision(
                Guid.NewGuid(),
                employeeId,
                decisionId,
                description,
                validFrom,
                validTo,
                status,
                notes
            );

            return Result<EmployeeDecision>.Success(decision);
        }
        public Guid EmployeeId { get; private set; }
        public Guid DecisionId { get; private set; } // Foreign Key to Settings/Lookups
        public string Description { get; private set; }
        public DateTime? ValidFrom { get; private set; }
        public DateTime? ValidTo { get; set; }
        public bool IsActive =>
            Status == EmployeeDecisionStatus.Active &&
            (!ValidTo.HasValue || ValidTo > DateTime.UtcNow);
        public EmployeeDecisionStatus Status { get; set; } // Active, Ended, Cancelled
        public string Notes { get; set; }

        public Result Cancel(string reason)
        {
            if (Status == EmployeeDecisionStatus.Cancelled)
                return Result.Failure(DecisionErrors.DecisionAlreadyCancelled);

            Status = EmployeeDecisionStatus.Cancelled;
            Notes = reason;

            return Result.Success();
        }

        public Result End(DateTime endDate)
        {
            if (Status == EmployeeDecisionStatus.Ended)
                return Result.Failure(DecisionErrors.DecisionAlreadyEnded);

            if (ValidFrom.HasValue && endDate < ValidFrom)
                return Result.Failure(DecisionErrors.InvalidEndDate);

            ValidTo = endDate;
            Status = EmployeeDecisionStatus.Ended;

            return Result.Success();
        }
    }
}
