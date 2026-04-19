using erp_kfs.Host.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyERP.Web.Models
{
    public class EmployeeAdmin
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الأب مطلوب")]
        public string MiddleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        public string LastName { get; set; } = string.Empty;

        [Required, StringLength(14, MinimumLength = 14, ErrorMessage = "الرقم القومي يجب أن يكون 14 رقم")]
        public string NationalId { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; } = "ذكر";
        public string Mobile { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsDisabled { get; set; }

        // وظيفي
        [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
        public string JobTitle { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string FinancialGrade { get; set; } = string.Empty;
        public DateTime? AppointmentDate { get; set; }
        public DateTime? GradeDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string AppointmentType { get; set; } = string.Empty;

        // مالي
        public decimal? BasicSalary2019 { get; set; }
        public decimal? GrossSalary { get; set; }
        public string InsuranceNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public bool HasFellowshipFund { get; set; }
        public bool HasMutualAid { get; set; }

        // مدة الخدمة
        public int ServiceYears { get; set; } = 0;
        public int ServiceMonths { get; set; } = 0;
        public int ServiceDays { get; set; } = 0;

        // إنهاء الخدمة
        public bool IsTerminated { get; set; } = false;
        public string? TerminationReason { get; set; }
        public DateTime? TerminationDate { get; set; }

        // ملفات
        public string? ProfileImagePath { get; set; }

        // ✅ بيانات الحساب (Email فقط - بدون Password!)
        [EmailAddress]
        public string? Email { get; set; }

        [Range(0, 50)]
        public int InitialLeaveBalance { get; set; } = 7;

        // ✅ كلمة السر المؤقتة (للـ Form فقط - لا تُحفظ في الـ DB)
        [NotMapped]
        public string? TempPassword { get; set; }

        // إدارة
        public string? SelectedDepartmentId { get; set; }

        [ForeignKey("SelectedDepartmentId")]
        public virtual Department? Department { get; set; }

        // ربط Identity
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // ✅ الاسم الكامل
        [NotMapped]
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        // ✅ العلاقة مع الإجازات (بدون [NotMapped])
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

        // ✅ حساب رصيد الإجازات
        [NotMapped]
        public decimal UsedAnnualLeaveThisYear
        {
            get
            {
                if (LeaveRequests == null || !LeaveRequests.Any())
                    return 0;

                return LeaveRequests
                    .Where(lr => lr.LeaveType?.Name == "Annual"
                              && lr.Status == "HRApproved"
                              && lr.StartDate.HasValue
                              && lr.StartDate.Value.Year == DateTime.Now.Year)
                    .Sum(lr => lr.DaysRequested);
            }
        }

        [NotMapped]
        public decimal RemainingAnnualLeave => AnnualLeaveEntitlement - UsedAnnualLeaveThisYear;

        [NotMapped]
        public int AnnualLeaveEntitlement
        {
            get
            {
                if (AppointmentDate == null) return 0;
                int serviceYears = (DateTime.Now - AppointmentDate.Value).Days / 365;
                int age = BirthDate.HasValue ? (DateTime.Now - BirthDate.Value).Days / 365 : 0;

                if (IsDisabled) return 45;
                if (age >= 50) return 50;
                if (serviceYears > 10) return 30;
                if (serviceYears >= 1) return 21;
                return 0;
            }
        }
    }
}