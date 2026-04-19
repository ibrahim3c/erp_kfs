using System.ComponentModel.DataAnnotations;

namespace MyERP.Web.Models
{
    public class Permission
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Display(Name = "كود المهمة")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "اسم المهمة")]
        public string DisplayName { get; set; } = string.Empty;

        [Display(Name = "الفئة")]
        public string Category { get; set; } = "General";

        [Display(Name = "الوصف القانوني")]
        public string LegalDescription { get; set; } = string.Empty;

        [Display(Name = "مرجع قانوني")]
        public string LegalReference { get; set; } = string.Empty;
    }
}