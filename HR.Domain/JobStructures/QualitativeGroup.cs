using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace HR.Domain.JobStructures
{
    public class QualitativeGroup : Entity
    {
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<FunctionalGroup> FunctionalGroups { get; set; }
    }
}
