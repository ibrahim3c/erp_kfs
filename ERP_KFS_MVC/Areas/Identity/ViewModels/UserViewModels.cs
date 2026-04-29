using System.ComponentModel.DataAnnotations;

namespace ERP_KFS_MVC.Areas.Identity.ViewModels
{
    public class UserEditViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; } = string.Empty;

        public List<string> CurrentRoles { get; set; } = new();
        public List<global::Identity.Application.Dtos.RoleDto> AllRoles { get; set; } = new();

        public List<string>? SelectedRoles { get; set; }
    }

    public class UserListViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public bool IsActive { get; set; }
    }
}