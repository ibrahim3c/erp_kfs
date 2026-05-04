using Microsoft.AspNetCore.Http;
using Modules.Shared.Application.Messaging;
namespace HR.Application.Employees.CreateEmployee
{
    public sealed record CreateEmployeeCommand(
        // 1. Personal Information
        string FirstName,
        string FatherName,
        string LastName,
        string NationalId,
        DateTime DateOfBirth,
        string Gender,
        string Phone,
        string Email,
        string MaritalStatus,
        string Address,
        bool IsDisabled,

        // 2. Job Information
        Guid? OrgUnitId,
        Guid JobTitleId,
        Guid? JobGradeId,
        DateTime HireDate,
        DateTime? JobGradeDate,
        Guid? EmploymentTypeId,
        Guid? FunctionalGroupId,

        // 3. E-Files (Documents as IFormFile)
        IFormFile? ProfileImage,
        IFormFile? NationalIdCardFront,
        IFormFile? NationalIdCardBack,
        IFormFile? QualificationFile,
        IFormFile? BirthCertificate,
        IFormFile? MilitaryFile,
        IFormFile? ContractFile,
        IFormFile? PoliceClearance,
        IFormFile? MarriageDocument,

        // 4. Financial Information
        decimal? BasicSalary2019,
        decimal? GrossSalary,
        string InsuranceNumber,
        string BankName,
        string BankAccountNumber,
        bool HasFellowshipFund,
        bool HasMedicalFund,

        // 5. Employee Qualification
        Guid QualificationTypeId,
        string QualificationFullName,
        string Specialization,
        string University,
        int? GraduationYear,
        string Grade,
        DateTime? QualificationValidFrom,
        DateTime? QualificationValidTo,
        string QualificationNotes
    ) : ICommand<Guid>;
}
