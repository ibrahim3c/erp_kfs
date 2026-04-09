using System;
using Modules.Shared.Domain;

namespace HR.Domain.Admin
{
    public sealed class AcademicIncentiveType : Entity
    {
        private readonly List<AcademicIncentiveRequest> _academicIncentiveRequests = new();

        private AcademicIncentiveType() { }

        private AcademicIncentiveType(
            Guid id,
            string code,
            string name,
            string scientificDegree,
            bool isPercentage,
            bool isFixedValue,
            decimal value,
            Guid? decisionId,
            bool isActive) : base(id)
        {
            Code = code;
            Name = name;
            ScientificDegree = scientificDegree;
            IsPercentage = isPercentage;
            IsFixedValue = isFixedValue;
            Value = value;
            DecisionId = decisionId;
            IsActive = isActive;
        }

        public string Code { get; private set; }

        public string Name { get; private set; }

        public string ScientificDegree { get; private set; }

        public bool IsPercentage { get; private set; }

        public bool IsFixedValue { get; private set; }

        public decimal Value { get; private set; }

        public Guid? DecisionId { get; private set; }

        public bool IsActive { get; private set; }

        // Navigation
        public Decision Decision { get; private set; }

        public IReadOnlyCollection<AcademicIncentiveRequest> AcademicIncentiveRequests =>
            _academicIncentiveRequests.AsReadOnly();

        public static Result<AcademicIncentiveType> Create(
            string code,
            string name,
            string scientificDegree,
            bool isPercentage,
            bool isFixedValue,
            decimal value,
            Guid? decisionId)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<AcademicIncentiveType>.Failure(AdminErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<AcademicIncentiveType>.Failure(AdminErrors.NameEmpty);

            if (!isPercentage && !isFixedValue)
                return Result<AcademicIncentiveType>.Failure(AdminErrors.InvalidValueType);

            if (value <= 0)
                return Result<AcademicIncentiveType>.Failure(AdminErrors.InvalidValue);

            var incentiveType = new AcademicIncentiveType(
                Guid.NewGuid(),
                code,
                name,
                scientificDegree,
                isPercentage,
                isFixedValue,
                value,
                decisionId,
                true);

            return Result<AcademicIncentiveType>.Success(incentiveType);
        }

        // Business Behaviors

        public Result UpdateDetails(
            string name,
            string scientificDegree,
            decimal value)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(AdminErrors.NameEmpty);

            if (value <= 0)
                return Result.Failure(AdminErrors.InvalidValue);

            Name = name;
            ScientificDegree = scientificDegree;
            Value = value;

            return Result.Success();
        }

        public Result Deactivate()
        {
            if (!IsActive)
                return Result.Failure(AdminErrors.AlreadyInactive);

            IsActive = false;

            return Result.Success();
        }

        public Result Activate()
        {
            if (IsActive)
                return Result.Failure(AdminErrors.AlreadyActive);

            IsActive = true;

            return Result.Success();
        }
    }
}

