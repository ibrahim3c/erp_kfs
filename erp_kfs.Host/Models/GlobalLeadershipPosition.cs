using erp_kfs.Host.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyERP.Web.Models
{
    public class GlobalLeadershipPosition
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString(); // غير من int إلى string

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        public int Level { get; set; } = 1;

        [Required]
        public string DepartmentId { get; set; } = "Governorate";

        public bool IsActive { get; set; } = true;

        // العلاقات العكسية
        public virtual ICollection<LeadershipAssignment> Assignments { get; set; } = new List<LeadershipAssignment>();
    
    public virtual Department? Department { get; set; }
    }
}