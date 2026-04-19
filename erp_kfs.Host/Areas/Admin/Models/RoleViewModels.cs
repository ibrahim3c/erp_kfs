using System.ComponentModel.DataAnnotations;
using erp_kfs.Host.Models;
using MyERP.Web.Models;

namespace MyERP.Web.Areas.Admin.Models
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "اسم الدور مطلوب")]
        [Display(Name = "اسم الدور")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم الدور من 2 إلى 50 حرف")]
        [RegularExpression(@"^[a-zA-Zأ-ي\s]+$", ErrorMessage = "اسم الدور يجب أن يحتوي على حروف فقط")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }
    }

    public class EditRoleViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الدور مطلوب")]
        [Display(Name = "اسم الدور")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }
    }

    public class RoleStatsViewModel
    {
        public int UserCount { get; set; }
        public int PermissionCount { get; set; }
    }

    public class RoleUsersViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<ApplicationUser> UsersInRole { get; set; } = new();
        public List<ApplicationUser> AllUsers { get; set; } = new();
    }
}