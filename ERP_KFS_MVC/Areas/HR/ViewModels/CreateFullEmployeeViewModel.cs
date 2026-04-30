using System.ComponentModel.DataAnnotations;

namespace ERP_KFS_MVC.Areas.HR.ViewModels
{
    public sealed class CreateFullEmployeeViewModel
    {
        // ── 1. البيانات الشخصية ──────────────────────────────────
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الأب مطلوب")]
        [MaxLength(50)]
        public string FatherName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "الرقم القومي 14 رقم بالضبط")]
        public string NationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "النوع مطلوب")]
        public string Gender { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? MaritalStatus { get; set; }
        public bool IsDisabled { get; set; }

        // ── 2. بيانات الوظيفة ────────────────────────────────────
        public Guid? OrgUnitId { get; set; }

        [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
        [MaxLength(150)]
        public string JobTitleName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? QualificationName { get; set; }

        public Guid? JobGradeId { get; set; }
        public Guid? EmploymentTypeId { get; set; }

        [Required(ErrorMessage = "تاريخ التعيين مطلوب")]
        public DateTime HireDate { get; set; }

        public DateTime? JobGradeDate { get; set; }

        // ── 3. الملفات ───────────────────────────────────────────
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? NationalIdCard { get; set; }
        public IFormFile? QualificationFile { get; set; }
        public IFormFile? BirthCertificate { get; set; }
        public IFormFile? MilitaryFile { get; set; }
        public IFormFile? ContractFile { get; set; }
        public IFormFile? PoliceClearance { get; set; }

        // ── 4. البيانات المالية ──────────────────────────────────
        public decimal? BasicSalary2019 { get; set; }
        public decimal? GrossSalary { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public bool HasFellowshipFund { get; set; }
        public bool HasMedicalFund { get; set; }
    }
}
