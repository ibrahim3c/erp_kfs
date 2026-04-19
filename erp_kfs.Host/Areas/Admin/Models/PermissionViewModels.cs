using System.ComponentModel.DataAnnotations;

namespace MyERP.Web.Areas.Admin.Models
{
    public class CreatePermissionViewModel
    {
        [Required(ErrorMessage = "كود الصلاحية مطلوب")]
        [Display(Name = "كود الصلاحية")]
        [RegularExpression(@"^[A-Za-z]+\.[A-Za-z]+$", ErrorMessage = "الكود يجب أن يكون بصيغة: Category.Action مثل Employees.Create")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العرض مطلوب")]
        [Display(Name = "اسم الصلاحية (عربي)")]
        public string DisplayName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الفئة مطلوبة")]
        [Display(Name = "الفئة")]
        public string Category { get; set; } = "General";

        [Display(Name = "فئة جديدة (اختياري)")]
        public string? NewCategory { get; set; }

        [Display(Name = "الوصف القانوني")]
        public string? LegalDescription { get; set; }

        [Display(Name = "المرجع القانوني")]
        public string? LegalReference { get; set; }
    }

    public class EditPermissionViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "كود الصلاحية مطلوب")]
        [Display(Name = "كود الصلاحية")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العرض مطلوب")]
        [Display(Name = "اسم الصلاحية (عربي)")]
        public string DisplayName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الفئة مطلوبة")]
        [Display(Name = "الفئة")]
        public string Category { get; set; } = "General";

        [Display(Name = "فئة جديدة (اختياري)")]
        public string? NewCategory { get; set; }

        [Display(Name = "الوصف القانوني")]
        public string? LegalDescription { get; set; }

        [Display(Name = "المرجع القانوني")]
        public string? LegalReference { get; set; }
    }

    public class PermissionStatsViewModel
    {
        public int RoleCount { get; set; }
        public int UserCount { get; set; }
    }
}