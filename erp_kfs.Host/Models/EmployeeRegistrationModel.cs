// Models/EmployeeRegistrationModel.cs
using System.ComponentModel.DataAnnotations;

namespace MyERP.Web.Models
{
    public class EmployeeRegistrationModel
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "كلمة المرور من 4 إلى 100 حرف")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "رصيد الإجازات الابتدائي")]
        public int InitialLeaveBalance { get; set; } = 7;
    }
}