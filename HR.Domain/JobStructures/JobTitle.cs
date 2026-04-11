using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HR.Domain.JobStructures
{
    public class JobTitle : Entity
    {
        [Required]
        public int FunctionalGroupId { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [ForeignKey(nameof(FunctionalGroupId))]
        public FunctionalGroup FunctionalGroup { get; set; }
    }
}
