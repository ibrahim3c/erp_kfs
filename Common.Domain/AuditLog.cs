using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace Common.Domain
{
    public class AuditLog : Entity
    {
        [Required]
        public string TableName { get; set; }

        [Required]
        public string RecordId { get; set; }

        [Required]
        public string ActionType { get; set; } // CREATE, UPDATE, DELETE

        public string ChangeDetails { get; set; } // XML
        public int? ChangedByUserId { get; set; }
        public DateTime ChangeTimestamp { get; set; }
    }
}
