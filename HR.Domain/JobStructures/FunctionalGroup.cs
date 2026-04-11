using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Domain.JobStructures
{
    public class FunctionalGroup : Entity
    {
        [Required]
        public int QualitativeGroupId { get; set; }

        [ForeignKey("QualitativeGroupId")]
        public virtual QualitativeGroup QualitativeGroup { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<JobTitle> JobTitles { get; set; }
    }
}
