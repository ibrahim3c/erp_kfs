namespace HR.Application.Employees.GetEmployeeForEdit
{

    public sealed class GetEmployeeForEditResponse
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
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
        public DateTime? JobGradeDate { get; init; }
        public DateTime CreatedAt { get; init; }

        public Guid? OrgUnitId { get; init; }
        public Guid? JobGradeId { get; init; }
        public Guid? EmploymentTypeId { get; init; }
        public Guid? FunctionalGroupId { get; init; }

        public Guid? JobTitleId { get; init; }         // أضفه عشان الـ dropdown
        public string? JobTitleName { get; init; }
        public string? QualificationTypeName { get; init; }

        // Financial
        public decimal? GrossSalary { get; init; }
        public decimal? BasicSalary2019 { get; init; }
        public string? InsuranceNumber { get; init; }
        public string? BankName { get; init; }
        public string? BankAccountNumber { get; init; }
        public bool HasFellowshipFund { get; init; }
        public bool HasMedicalFund { get; init; }
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
