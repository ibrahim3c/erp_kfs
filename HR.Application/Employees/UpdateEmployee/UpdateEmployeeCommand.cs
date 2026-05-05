using Microsoft.AspNetCore.Http;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.UpdateEmployee
{
    public sealed record UpdateEmployeeCommand(
     Guid Id,
     string Name,
     string Code,
     string? Phone,
     string? Email,
     string? Gender,
     string? Address,
     string? MaritalStatus,
     DateTime? DateOfBirth,
     DateTime HireDate,
     DateTime? JobGradeDate,
     bool IsDisabled,
     Guid? OrgUnitId,
     Guid? JobTitleId,
     Guid? JobGradeId,
     Guid? EmploymentTypeId,
     Guid? FunctionalGroupId,
     decimal? GrossSalary,
     decimal? BasicSalary2019,
     string? InsuranceNumber,
     string? BankName,
     string? BankAccountNumber,
     bool HasFellowshipFund,
     bool HasMedicalFund,
     // الملفات الجديدة
     IFormFile? ProfileImage,
     IFormFile? NationalIdCardFront,
     IFormFile? NationalIdCardBack,
     IFormFile? QualificationFile,
     IFormFile? BirthCertificate,
     IFormFile? MilitaryFile,
     IFormFile? ContractFile,
     IFormFile? PoliceClearance,
     IFormFile? MarriageDocument,
     // الملفات الحالية (fallback)
     string? CurrentPersonalPhoto,
     string? CurrentNationalIdCardFront,
     string? CurrentNationalIdCardBack,
     string? CurrentQualificationFile,
     string? CurrentBirthCertificateFile,
     string? CurrentMilitaryFile,
     string? CurrentContractFile,
     string? CurrentPoliceClearance,
     string? CurrentMarriageDocument
 ) : ICommand;
}
