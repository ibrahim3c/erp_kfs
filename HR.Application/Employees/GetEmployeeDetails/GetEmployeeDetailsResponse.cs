namespace HR.Application.Employees.GetEmployeeDetails
{
    public sealed class GetEmployeeDetailsResponse
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string FatherName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string? Phone { get; init; }
        public string? Email { get; init; }
        public string? NationalId { get; init; }
        public string? Gender { get; init; }
        public string? Address { get; init; }
        public string? MaritalStatus { get; init; }
        public bool IsActive { get; init; }
        public bool IsDisabled { get; init; }
        public DateTime HireDate { get; init; }
        public DateTime? DateOfBirth { get; init; }
        //public DateTime? JobGradeDate { get; init; }


        // Job
        public string? JobTitleName { get; init; }
        public string? JobGradeName { get; init; }
        public string? EmploymentTypeName { get; init; }
        public string? OrgUnitName { get; init; }
        public string? FunctionalGroupName { get; init; }

        // Qualification
        public Guid? QualificationTypeId { get; init; }
        public string? QualificationTypeName { get; init; }
        public string? QualificationFullName { get; init; }
        public string? QualificationSpecialization { get; init; }
        public string? QualificationUniversity { get; init; }
        public int? QualificationGraduationYear { get; init; }
        public string? QualificationGrade { get; init; }
        public bool? QualificationIsVerified { get; init; }
        public DateTime? QualificationValidFrom { get; init; }
        public DateTime? QualificationValidTo { get; init; }
        public string? QualificationNotes { get; init; }

        // Financial
        public decimal? GrossSalary { get; init; }
        public decimal? BasicSalary2019 { get; init; }
        public string? InsuranceNumber { get; init; }
        public string? BankName { get; init; }
        public string? BankAccount { get; init; }
        public bool HasFellowshipFund { get; init; }
        public bool HasMedicalFund { get; init; }
        // Qualification
        public string? QualificationFullName { get; init; }
        public string? QualificationTypeName { get; init; }
        public string? Specialization { get; init; }
        public string? University { get; init; }
        public int? GraduationYear { get; init; }
        public string? Grade { get; init; }
        public DateTime? QualificationValidFrom { get; init; }
        public DateTime? QualificationValidTo { get; init; }
        public string? QualificationNotes { get; init; }

        // Files
        public string? PersonalPhoto { get; init; }
        public string? NationalIdCardFront { get; init; }
        public string? NationalIdCardBack { get; init; }
        public string? QualificationFile { get; init; }
        public string? BirthCertificateFile { get; init; }
        public string? MilitaryFile { get; init; }
        public string? ContractFile { get; init; }
        public string? PoliceClearanceCertificate { get; init; }
        public string? MarriageDocument { get; init; }
    }
}
