using Modules.Shared.Domain;

namespace Organization.Domain
{
    public class JobTitle : Entity
    {
        private JobTitle() { }

        private JobTitle(Guid id, Guid functionalGroupId, string code, string name, string description, bool isActive) : base(id)
        {
            FunctionalGroupId = functionalGroupId;
            Code = code;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid FunctionalGroupId { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public FunctionalGroup FunctionalGroup { get; private set; }

        public static Result<JobTitle> Create(Guid functionalGroupId, string code, string name, string description)
        {
            if (functionalGroupId == Guid.Empty)
                return Result<JobTitle>.Failure(OrganizationErrors.FunctionalGroupIdEmpty);

            if (string.IsNullOrWhiteSpace(code))
                return Result<JobTitle>.Failure(OrganizationErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<JobTitle>.Failure(OrganizationErrors.NameEmpty);

            var jobTitle = new JobTitle(Guid.NewGuid(), functionalGroupId, code, name, description, true);
            return Result<JobTitle>.Success(jobTitle);
        }

        public Result UpdateDetails(string code, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(code)) return Result.Failure(OrganizationErrors.CodeEmpty);
            if (string.IsNullOrWhiteSpace(name)) return Result.Failure(OrganizationErrors.NameEmpty);

            Code = code;
            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result Activate() { IsActive = true; UpdatedAt = DateTime.UtcNow; return Result.Success(); }
        public Result Deactivate() { IsActive = false; UpdatedAt = DateTime.UtcNow; return Result.Success(); }
    }
}