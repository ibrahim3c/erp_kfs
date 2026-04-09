using Modules.Shared.Domain;

namespace HR.Domain.Employees.Qualifications
{
    public sealed class EmployeeQualification : Entity
    {
        private EmployeeQualification() { }

        private EmployeeQualification(
            Guid id,
            Guid employeeId,
            Guid qualificationTypeId,
            string qualificationFullName,
            string specialization,
            string university,
            int? graduationYear,
            string grade,
            string filePath,
            DateTime? validFrom,
            DateTime? validTo,
            string notes) : base(id)
        {
            EmployeeId = employeeId;
            QualificationTypeId = qualificationTypeId;
            QualificationFullName = qualificationFullName;
            Specialization = specialization;
            University = university;
            GraduationYear = graduationYear;
            Grade = grade;
            FilePath = filePath;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Notes = notes;
            IsVerified = false;
        }

        public Guid EmployeeId { get; private set; }

        public Guid QualificationTypeId { get; private set; }

        public string QualificationFullName { get; private set; }

        public string Specialization { get; private set; }

        public string University { get; private set; }

        public int? GraduationYear { get; private set; }

        public string Grade { get; private set; }

        public string FilePath { get; private set; }

        public bool IsVerified { get; private set; }

        public DateTime? ValidFrom { get; private set; }

        public DateTime? ValidTo { get; private set; }

        public string Notes { get; private set; }

        // Navigation
        public Employee Employee { get; private set; }

        // -------------------------
        // Factory
        // -------------------------

        public static Result<EmployeeQualification> Create(
            Guid employeeId,
            Guid qualificationTypeId,
            string qualificationFullName,
            string specialization = null,
            string university = null,
            int? graduationYear = null,
            string grade = null,
            string filePath = null,
            DateTime? validFrom = null,
            DateTime? validTo = null,
            string notes = null)
        {
            if (employeeId == Guid.Empty)
                return Result<EmployeeQualification>.Failure(EmployeeErrors.EmployeeIdEmpty);

            if (qualificationTypeId == Guid.Empty)
                return Result<EmployeeQualification>.Failure(EmployeeErrors.QualificationIdEmpty);

            if (string.IsNullOrWhiteSpace(qualificationFullName))
                return Result<EmployeeQualification>.Failure(EmployeeErrors.QualificationFullNameEmpty);

            if (validTo.HasValue && validFrom.HasValue && validTo < validFrom)
                return Result<EmployeeQualification>.Failure(EmployeeErrors.InvalidQualificationDates);

            var qualification = new EmployeeQualification(
                Guid.NewGuid(),
                employeeId,
                qualificationTypeId,
                qualificationFullName,
                specialization,
                university,
                graduationYear,
                grade,
                filePath,
                validFrom,
                validTo,
                notes
            );

            return Result<EmployeeQualification>.Success(qualification);
        }

        // -------------------------
        // Business Behaviors
        // -------------------------

        public Result Verify()
        {
            if (IsVerified)
                return Result.Failure(EmployeeErrors.AlreadyVerifiedQualification);

            IsVerified = true;
            return Result.Success();
        }

        public Result Update(
            string qualificationFullName,
            string specialization = null,
            string university = null,
            int? graduationYear = null,
            string grade = null,
            DateTime? validFrom = null,
            DateTime? validTo = null,
            string notes = null)
        {
            if (string.IsNullOrWhiteSpace(qualificationFullName))
                return Result.Failure(EmployeeErrors.QualificationFullNameEmpty);

            if (validTo.HasValue && validFrom.HasValue && validTo < validFrom)
                return Result.Failure(EmployeeErrors.InvalidQualificationDates);

            QualificationFullName = qualificationFullName;
            Specialization = specialization;
            University = university;
            GraduationYear = graduationYear;
            Grade = grade;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Notes = notes;

            return Result.Success();
        }
    }
}
