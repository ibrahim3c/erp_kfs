using Modules.Shared.Domain;

namespace HR.Domain.Decisions
{
    public sealed class DecisionAuthority : Entity
    {
        private readonly List<Decision> _decisions = new();

        private DecisionAuthority() { }

        private DecisionAuthority(
            Guid id,
            string name,
            string description,
            bool isActive) : base(id)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool IsActive { get; private set; } = true;

        public IReadOnlyCollection<Decision> Decisions => _decisions.AsReadOnly();

        // ------------------------
        // Factory
        // ------------------------

        public static Result<DecisionAuthority> Create(
            string name,
            string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<DecisionAuthority>.Failure(DecisionErrors.AuthorityNameEmpty);

            var authority = new DecisionAuthority(
                Guid.NewGuid(),
                name,
                description,
                true);

            return Result<DecisionAuthority>.Success(authority);
        }

        // ------------------------
        // Business Behaviors
        // ------------------------

        public Result UpdateDetails(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(DecisionErrors.AuthorityNameEmpty);

            Name = name;
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

    }
}
