using Modules.Shared.Domain;

namespace HR.Domain.Employees.Incentives
{
    public sealed class AcademicIncentiveType : Entity
    {
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
                return Result<AcademicIncentiveType>.Failure(EmployeeErrors.IncentiveCodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<AcademicIncentiveType>.Failure(EmployeeErrors.IncentiveNameEmpty);

            if (!isPercentage && !isFixedValue)
                return Result<AcademicIncentiveType>.Failure(EmployeeErrors.IncentiveInvalidValueType);

            if (value <= 0)
                return Result<AcademicIncentiveType>.Failure(EmployeeErrors.IncentiveInvalidValue);

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
                return Result.Failure(EmployeeErrors.IncentiveCodeEmpty);

            if (value <= 0)
                return Result.Failure(EmployeeErrors.IncentiveInvalidValue);

            Name = name;
            ScientificDegree = scientificDegree;
            Value = value;

            return Result.Success();
        }

        public Result Deactivate()
        {
            if (!IsActive)
                return Result.Failure(EmployeeErrors.IncentiveAlreadyInactive);

            IsActive = false;

            return Result.Success();
        }

        public Result Activate()
        {
            if (IsActive)
                return Result.Failure(EmployeeErrors.IncentiveAlreadyActive);

            IsActive = true;

            return Result.Success();
        }
    }
}

