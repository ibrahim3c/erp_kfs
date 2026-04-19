using MyERP.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace erp_kfs.Host.Models
{
    public class Department
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "اسم الإدارة مطلوب")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = "Sub"; // General أو Sub

        public string? ParentId { get; set; }
        public Department? Parent { get; set; }
        public ICollection<Department> Children { get; set; } = new List<Department>();

        public string? ManagerId { get; set; }
        public virtual EmployeeAdmin? Manager { get; set; }
    }
}