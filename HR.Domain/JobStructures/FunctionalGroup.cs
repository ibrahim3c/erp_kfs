using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Domain.JobStructures
{
    public class FunctionalGroup : Entity
    {
        private readonly List<JobTitle> _jobTitles = new();

        private FunctionalGroup() { }

        private FunctionalGroup(Guid id, Guid qualitativeGroupId, string code, string name, string description, bool isActive) : base(id)
        {
            QualitativeGroupId = qualitativeGroupId;
            Code = code;
            Name = name;
            Description = description;
            IsActive = isActive;
        }

        public Guid QualitativeGroupId { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation
        public QualitativeGroup QualitativeGroup { get; private set; }
        public IReadOnlyCollection<JobTitle> JobTitles => _jobTitles.AsReadOnly();

        // Factory
        public static Result<FunctionalGroup> Create(Guid qualitativeGroupId, string code, string name, string description)
        {
            if (qualitativeGroupId == Guid.Empty)
                return Result<FunctionalGroup>.Failure(JobStructureErrors.QualitativeGroupIdEmpty);

            if (string.IsNullOrWhiteSpace(code))
                return Result<FunctionalGroup>.Failure(JobStructureErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<FunctionalGroup>.Failure(JobStructureErrors.NameEmpty);

            var group = new FunctionalGroup(Guid.NewGuid(), qualitativeGroupId, code, name, description, true);
            return Result<FunctionalGroup>.Success(group);
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

        public Result Activate() {
            IsActive = true;
            return Result.Success(); 
        }
        public Result Deactivate() {
            IsActive = false;
            return Result.Success(); 
        }
    }
}
