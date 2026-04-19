using Modules.Shared.Domain;

namespace HR.Domain.Employees.Qualifications
{
        public sealed class QualificationType : Entity
        {
            private QualificationType() { }

            private QualificationType(Guid id, string name, string description, bool isActive) : base(id)
            {
                Name = name;
                Description = description;
                IsActive = isActive;
            }

            public string Name { get; private set; }

            public string Description { get; private set; }

            public bool IsActive { get; private set; }

            public static Result<QualificationType> Create(string name, string description)
            {
                if (string.IsNullOrWhiteSpace(name))
                    return Result<QualificationType>.Failure(EmployeeErrors.NameEmpty);

                var qualificationType = new QualificationType(
                    Guid.NewGuid(),
                    name,
                    description,
                    true);

                return Result<QualificationType>.Success(qualificationType);
            }

            // Business Behaviors
            public Result UpdateDetails(string name, string description)
            {
                if (string.IsNullOrWhiteSpace(name))
                    return Result.Failure(EmployeeErrors.NameEmpty);

                Name = name;
                Description = description;

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