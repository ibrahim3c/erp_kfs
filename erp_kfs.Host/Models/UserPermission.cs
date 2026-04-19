// Models/UserPermission.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using erp_kfs.Host.Models;
using Microsoft.AspNetCore.Identity;

namespace MyERP.Web.Models
{
    public class UserPermission
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string PermissionId { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; } = null!;
    }
}