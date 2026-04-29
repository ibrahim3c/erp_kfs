using System.ComponentModel.DataAnnotations;

namespace ERP_KFS_MVC.Areas.Identity.ViewModels
{
    public class AssignPermissionsViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<PermissionItem> Permissions { get; set; } = new();
        public string[]? SelectedPermissionIds { get; set; }
    }

    public class PermissionItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}