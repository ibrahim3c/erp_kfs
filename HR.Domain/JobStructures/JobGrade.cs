using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;
namespace HR.Domain.JobStructures
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

        // Factory
        public static Result<JobGrade> Create(string code, string name, int gradeLevel, string description, int yearsNo)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<JobGrade>.Failure(JobStructureErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<JobGrade>.Failure(JobStructureErrors.NameEmpty);

            if (gradeLevel <= 0)
                return Result<JobGrade>.Failure(JobStructureErrors.InvalidGradeLevel);

            if (yearsNo < 0)
                return Result<JobGrade>.Failure(JobStructureErrors.InvalidYearsNo);

            var jobGrade = new JobGrade(Guid.NewGuid(), code, name, gradeLevel, description, yearsNo, true);
            return Result<JobGrade>.Success(jobGrade);
        }

        // Behaviors
        public Result UpdateDetails(string code, string name, int gradeLevel, string description, int yearsNo)
        {
            if (string.IsNullOrWhiteSpace(code)) return Result.Failure(JobStructureErrors.CodeEmpty);
            if (string.IsNullOrWhiteSpace(name)) return Result.Failure(JobStructureErrors.NameEmpty);
            if (gradeLevel <= 0) return Result.Failure(JobStructureErrors.InvalidGradeLevel);
            if (yearsNo < 0) return Result.Failure(JobStructureErrors.InvalidYearsNo);

            Code = code;
            Name = name;
            GradeLevel = gradeLevel;
            Description = description;
            YearsNo = yearsNo;

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
