using MyERP.Web.Models;

namespace erp_kfs.Host.Models
{
    public class AssignUserPermissionsViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = new();
        public List<string> SelectedPermissionIds { get; set; } = new();
    }
}