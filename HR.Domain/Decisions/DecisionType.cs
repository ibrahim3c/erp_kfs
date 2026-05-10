using Modules.Shared.Domain;

namespace HR.Domain.Decisions
{
    public sealed class DecisionType : Entity
    {
        private DecisionType() { }

        private DecisionType(
            Guid id,
            string code,
            string description,
            bool affectsEmploymentType,
            bool affectsSalary,
            bool affectsPosition,
            bool hasEndDate,
            bool isActive) : base(id)
        {
            Code = code;
            Description = description;
            AffectsEmploymentType = affectsEmploymentType;
            AffectsSalary = affectsSalary;
            AffectsPosition = affectsPosition;
            HasEndDate = hasEndDate;
            IsActive = isActive;
        }

        public string Code { get; private set; }

        public string Description { get; private set; }

        public bool AffectsEmploymentType { get; private set; }

        public bool AffectsSalary { get; private set; }

        public bool AffectsPosition { get; private set; }

        public bool HasEndDate { get; private set; }

        public bool IsActive { get; private set; }

        // ---------------------------
        // Factory
        // ---------------------------

        public static Result<DecisionType> Create(
            string code,
            string description,
            bool affectsEmploymentType,
            bool affectsSalary,
            bool affectsPosition,
            bool hasEndDate)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<DecisionType>.Failure(DecisionErrors.CodeEmpty);

            var decisionType = new DecisionType(
                Guid.NewGuid(),
                code,
                description,
                affectsEmploymentType,
                affectsSalary,
                affectsPosition,
                hasEndDate,
                true
            );

            return Result<DecisionType>.Success(decisionType);
        }

        // ---------------------------
        // Business Behaviors
        // ---------------------------

        public Result UpdateDetails(
            string code,
            string description)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result.Failure(DecisionErrors.CodeEmpty);

            Code = code;
            Description = description;

            return Result.Success();
        }

        public Result Activate()
        {
            if (IsActive)
                return Result.Failure(DecisionErrors.AlreadyActive);

            IsActive = true;

            return Result.Success();
        }

        public Result Deactivate()
        {
            if (!IsActive)
                return Result.Failure(DecisionErrors.AlreadyInactive);

            IsActive = false;

            return Result.Success();
        }

        // ---------------------------
        // Business Rules Helpers
        // ---------------------------

        public bool IsSalaryImpacting()
        {
            return AffectsSalary;
        }

        public bool IsPositionChanging()
        {
            return AffectsPosition;
        }

        public bool IsEmploymentChanging()
        {
            return AffectsEmploymentType;
        }
    }
}
