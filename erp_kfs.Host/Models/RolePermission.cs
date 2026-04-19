// Models/RolePermission.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyERP.Web.Models
{
    public class RolePermission
    {
        // ⚠️ لا يوجد Id هنا - المفتاح مركب (RoleId + PermissionId)
        
        [Required]
        public string RoleId { get; set; } = string.Empty;
        
        [Required]
        public string PermissionId { get; set; } = string.Empty;
        
        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; } = null!;
        
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; } = null!;
    }
}