using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Modules.Shared.Domain;

namespace Common.Domain
{
    public class Notification : Entity
    {

        [Required]
        public int SentTo { get; set; }

        [Required]
        public int SentBy { get; set; }

        [Required]
        public string Text { get; set; }

      // Navigation
        [ForeignKey(nameof(SentTo))]
        public Employee SentToEmployee { get; set; }

        [ForeignKey(nameof(SentBy))]
        public Employee SentByEmployee { get; set; }
    }
}
