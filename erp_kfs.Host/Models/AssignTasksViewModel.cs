using MyERP.Web.Models;

namespace erp_kfs.Host.Models
{
    public class AssignTasksViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = new();
        public string[]? SelectedTaskIds { get; set; }
    }
}