using System.ComponentModel.DataAnnotations;

namespace ERP_KFS_MVC.Areas.Identity.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "اسم الدور مطلوب")]
        [Display(Name = "اسم الدور")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم الدور من 2 إلى 50 حرف")]
        public string Name { get; set; } = string.Empty;
    }

    public class EditRoleViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "اسم الدور مطلوب")]
        [Display(Name = "اسم الدور")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }

    public class RoleStatsViewModel
    {
        public int UserCount { get; set; }
        public int PermissionCount { get; set; }
    }

    public class RoleUsersViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<UserViewModel> UsersInRole { get; set; } = new();
        public List<UserViewModel> AllUsers { get; set; } = new();
    }

    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}