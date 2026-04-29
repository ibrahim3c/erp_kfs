using Modules.Shared.Domain;

namespace Organization.Domain
{
    public class JobGrade : Entity
    {
        private JobGrade() { }

        private JobGrade(Guid id, string code, string name, int gradeLevel, string description, int yearsNo, bool isActive) : base(id)
        {
            Code = code;
            Name = name;
            GradeLevel = gradeLevel;
            Description = description;
            YearsNo = yearsNo;
            IsActive = isActive;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }
        public int GradeLevel { get; private set; }
        public string Description { get; private set; }
        public int YearsNo { get; private set; }
        public bool IsActive { get; private set; }

        public static Result<JobGrade> Create(string code, string name, int gradeLevel, string description, int yearsNo)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<JobGrade>.Failure(OrganizationErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<JobGrade>.Failure(OrganizationErrors.NameEmpty);

            if (gradeLevel <= 0)
                return Result<JobGrade>.Failure(OrganizationErrors.InvalidGradeLevel);

            if (yearsNo < 0)
                return Result<JobGrade>.Failure(OrganizationErrors.InvalidYearsNo);

            var jobGrade = new JobGrade(Guid.NewGuid(), code, name, gradeLevel, description, yearsNo, true);
            return Result<JobGrade>.Success(jobGrade);
        }

        public Result UpdateDetails(string code, string name, int gradeLevel, string description, int yearsNo)
        {
            if (string.IsNullOrWhiteSpace(code)) return Result.Failure(OrganizationErrors.CodeEmpty);
            if (string.IsNullOrWhiteSpace(name)) return Result.Failure(OrganizationErrors.NameEmpty);
            if (gradeLevel <= 0) return Result.Failure(OrganizationErrors.InvalidGradeLevel);
            if (yearsNo < 0) return Result.Failure(OrganizationErrors.InvalidYearsNo);

            Code = code;
            Name = name;
            GradeLevel = gradeLevel;
            Description = description;
            YearsNo = yearsNo;

            return Result.Success();
        }

        public Result Activate() { IsActive = true; return Result.Success(); }
        public Result Deactivate() { IsActive = false; return Result.Success(); }
    }
}