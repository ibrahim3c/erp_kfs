using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace HR.Domain.JobStructures
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

        // Factory
        public static Result<QualitativeGroup> Create(string code, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<QualitativeGroup>.Failure(JobStructureErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<QualitativeGroup>.Failure(JobStructureErrors.NameEmpty);

            var group = new QualitativeGroup(Guid.NewGuid(), code, name, description, true);
            return Result<QualitativeGroup>.Success(group);
        }

        // Behaviors
        public Result UpdateDetails(string code, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(code)) return Result.Failure(JobStructureErrors.CodeEmpty);
            if (string.IsNullOrWhiteSpace(name)) return Result.Failure(JobStructureErrors.NameEmpty);

            Code = code;
            Name = name;
            Description = description;

            return Result.Success();
        }

        public Result Activate()
        {
            if (IsActive) return Result.Failure(JobStructureErrors.AlreadyActive);
            IsActive = true;
            return Result.Success();
        }

        public Result Deactivate()
        {
            if (!IsActive) return Result.Failure(JobStructureErrors.AlreadyInactive);
            IsActive = false;
            return Result.Success();
        }
    }
}
