using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyERP.Web.Areas.HR.Models;

namespace MyERP.Web.Models.Common
{
    public class Notification : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SentTo { get; set; }

        [Required]
        public string SentBy { get; set; }

        [Required]
        public string Text { get; set; }

      // Navigation
        [ForeignKey(nameof(SentTo))]
        public EmployeeAdmin SentToEmployee { get; set; }

        [ForeignKey("SentBy")]
        public EmployeeAdmin SentByEmployee { get; set; }
    }
}
