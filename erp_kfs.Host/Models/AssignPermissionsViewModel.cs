using MyERP.Web.Models;

namespace erp_kfs.Host.Models
{
    public class AssignPermissionsViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = new();
        public string[]? SelectedPermissionIds { get; set; }
    }
}