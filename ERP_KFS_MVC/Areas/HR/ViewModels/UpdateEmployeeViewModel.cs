using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ERP_KFS_MVC.Areas.HR.ViewModels
{
    public sealed class UpdateEmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "اسم الأب مطلوب")]
        public string FatherName { get; set; } = string.Empty;
        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        public string LastName { get; set; } = string.Empty;

        public string? NationalId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDisabled { get; set; }

        [Required(ErrorMessage = "تاريخ التعيين مطلوب")]
        public DateTime HireDate { get; set; }
        public DateTime? JobGradeDate { get; set; }

        public Guid? OrgUnitId { get; set; }
        public Guid? JobTitleId { get; set; }
        public Guid? JobGradeId { get; set; }
        public Guid? EmploymentTypeId { get; set; }
        public Guid? FunctionalGroupId { get; set; }

        public string? JobTitleName { get; set; }
        public string? QualificationName { get; set; }

        public decimal? GrossSalary { get; set; }
        public decimal? BasicSalary2019 { get; set; }
        public decimal? Incentives { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public bool HasFellowshipFund { get; set; }
        public bool HasMedicalFund { get; set; }

        // الملفات الحالية (للعرض)
        public string? CurrentPersonalPhoto { get; set; }
        public string? CurrentNationalIdCardFront { get; set; }
        public string? CurrentNationalIdCardBack { get; set; }
        public string? CurrentQualificationFile { get; set; }
        public string? CurrentBirthCertificateFile { get; set; }
        public string? CurrentMilitaryFile { get; set; }
        public string? CurrentContractFile { get; set; }
        public string? CurrentPoliceClearance { get; set; }
        public string? CurrentMarriageDocument { get; set; }

        // الملفات الجديدة (للرفع)
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? NationalIdCardFront { get; set; }
        public IFormFile? NationalIdCardBack { get; set; }
        public IFormFile? QualificationFile { get; set; }
        public IFormFile? BirthCertificate { get; set; }
        public IFormFile? MilitaryFile { get; set; }
        public IFormFile? ContractFile { get; set; }
        public IFormFile? PoliceClearance { get; set; }
        public IFormFile? MarriageDocument { get; set; }
    }
}