using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Geography.Domain
{
    public class Village:Entity
    {

        [Required]
        public int LocalUnitId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties


        [ForeignKey(nameof(LocalUnitId))]
        public LocalUnit LocalUnit { get; set; }
    }
}
