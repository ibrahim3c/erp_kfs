using Modules.Shared.Domain;

namespace Organization.Domain
{
    public class QualitativeGroup : Entity
    {
        private readonly List<FunctionalGroup> _functionalGroups = new();

        private QualitativeGroup() { }

        private QualitativeGroup(Guid id, string code, string name, string description, bool isActive) : base(id)
        {
            Code = code;
            Name = name;
            Description = description;
            IsActive = isActive;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<FunctionalGroup> FunctionalGroups => _functionalGroups.AsReadOnly();

        public static Result<QualitativeGroup> Create(string code, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<QualitativeGroup>.Failure(OrganizationErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<QualitativeGroup>.Failure(OrganizationErrors.NameEmpty);

            var group = new QualitativeGroup(Guid.NewGuid(), code, name, description, true);
            return Result<QualitativeGroup>.Success(group);
        }

        public Result UpdateDetails(string code, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(code)) return Result.Failure(OrganizationErrors.CodeEmpty);
            if (string.IsNullOrWhiteSpace(name)) return Result.Failure(OrganizationErrors.NameEmpty);

            Code = code;
            Name = name;
            Description = description;

            return Result.Success();
        }

        public Result Activate()
        {
            if (IsActive) return Result.Failure(OrganizationErrors.AlreadyActive);
            IsActive = true;
            return Result.Success();
        }

        public Result Deactivate()
        {
            if (!IsActive) return Result.Failure(OrganizationErrors.AlreadyInactive);
            IsActive = false;
            return Result.Success();
        }
    }
}