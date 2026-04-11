using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Geography.Domain
{
    public class LocalUnit : Entity
    {
        [Required]
        public int CityCenterId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(CityCenterId))]
        public CityCenter CityCenter { get; set; }

        public ICollection<Village> Villages { get; set; }

    }
}
