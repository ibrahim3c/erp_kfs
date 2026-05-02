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
        public bool IsActive { get; set; }
        public bool IsDisabled { get; set; }

        [Required(ErrorMessage = "تاريخ التعيين مطلوب")]
        public DateTime HireDate { get; set; }
        public DateTime? JobGradeDate { get; set; }

        public Guid? OrgUnitId { get; set; }
        public Guid? JobGradeId { get; set; }
        public Guid? EmploymentTypeId { get; set; }

        [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
        public string JobTitleName { get; set; } = string.Empty;
        public string? QualificationName { get; set; }

        public decimal? GrossSalary { get; set; }
        public decimal? BasicSalary2019 { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public bool HasFellowshipFund { get; set; }
        public bool HasMedicalFund { get; set; }
    }
}
