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

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")] // أضفنا Required بناءً على تصميم الواجهة
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? MaritalStatus { get; set; }
        public bool IsDisabled { get; set; }

        // ── 2. بيانات الوظيفة ────────────────────────────────────
        public Guid? OrgUnitId { get; set; }

        [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
        public Guid? JobTitleId { get; set; }

        // تم حذف QualificationName القديم من هنا واستبداله بالقسم رقم 3

        public Guid? JobGradeId { get; set; }

        // تم الاعتماد على EmploymentTypeId ليكون نوع الكادر (كما طلبت)
        public Guid? EmploymentTypeId { get; set; }

        [Required(ErrorMessage = "تاريخ التعيين مطلوب")]
        public DateTime HireDate { get; set; }

        public DateTime? JobGradeDate { get; set; }
        public Guid? FunctionalGroupId { get; set; }

        // ── 3. بيانات المؤهل الدراسي (القسم الجديد) ───────────────
        [Required(ErrorMessage = "نوع المؤهل مطلوب")]
        public Guid QualificationTypeId { get; set; }

        [Required(ErrorMessage = "اسم المؤهل بالكامل مطلوب")]
        [MaxLength(200, ErrorMessage = "اسم المؤهل يجب ألا يتجاوز 200 حرف")]
        public string QualificationFullName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "اسم التخصص يجب ألا يتجاوز 100 حرف")]
        public string? Specialization { get; set; }

        [MaxLength(150, ErrorMessage = "اسم الجامعة/المعهد يجب ألا يتجاوز 150 حرف")]
        public string? University { get; set; }

        public int? GraduationYear { get; set; }

        [MaxLength(50)]
        public string? Grade { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // ── 4. الملفات (مسوغات التعيين) ───────────────────────────
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? NationalIdCardFront { get; set; }
        public IFormFile? NationalIdCardBack { get; set; }
        public IFormFile? QualificationFile { get; set; }
        public IFormFile? BirthCertificate { get; set; }
        public IFormFile? MilitaryFile { get; set; }
        public IFormFile? ContractFile { get; set; }
        public IFormFile? PoliceClearance { get; set; }
        public IFormFile? MarriageDocument { get; set; }

        // ── 5. البيانات المالية ──────────────────────────────────
        public decimal? BasicSalary2019 { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? Incentives { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public bool HasFellowshipFund { get; set; }
        public bool HasMedicalFund { get; set; }
    }
}
