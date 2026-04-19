using HR.Domain.Employees;
using Modules.Shared.Domain;

namespace HR.Domain.Terminations
{
    public sealed class ServiceTerminationType : Entity
    {
        private ServiceTerminationType() { }

        private ServiceTerminationType(
            Guid id,
            string code,
            string name,
            string description,
            bool requiresNoticePeriod) : base(id)
        {
            Code = code;
            Name = name;
            Description = description;
            RequiresNoticePeriod = requiresNoticePeriod;
            IsActive = true;
        }

        public string Code { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool RequiresNoticePeriod { get; private set; }

        public bool IsActive { get; private set; }

        private readonly List<ServiceTerminationRequest> _serviceTerminationRequests = new();
        public IReadOnlyCollection<ServiceTerminationRequest> ServiceTerminationRequests => _serviceTerminationRequests.AsReadOnly();

        public static Result<ServiceTerminationType> Create(
            string code,
            string name,
            string description,
            bool requiresNoticePeriod)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<ServiceTerminationType>.Failure(EmployeeErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<ServiceTerminationType>.Failure(EmployeeErrors.NameEmpty);

            var terminationType = new ServiceTerminationType(
                Guid.NewGuid(),
                code,
                name,
                description,
                requiresNoticePeriod
            );

            return Result<ServiceTerminationType>.Success(terminationType);
        }

        // Business Behaviors

        public Result UpdateDetails(
            string name,
            string description,
            bool requiresNoticePeriod)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(EmployeeErrors.NameEmpty);

            Name = name;
            Description = description;
            RequiresNoticePeriod = requiresNoticePeriod;

            return Result.Success();
        }

        public Result Activate()
        {
            if (IsActive)
                return Result.Failure(EmployeeErrors.AlreadyActive);

            IsActive = true;

            return Result.Success();
        }

        public Result Deactivate()
        {
            if (!IsActive)
                return Result.Failure(EmployeeErrors.AlreadyInactive);

            IsActive = false;

            return Result.Success();
        }
    }
}
