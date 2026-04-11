using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace Geography.Domain
{
    public class CityCenter : Entity
    {
        [Required]
        public int GovernorateId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } // center | city



        // Navigation
        public Governorate Governorate { get; set; }
        public ICollection<LocalUnit> LocalUnits { get; set; }
        public ICollection<Village> Villages { get; set; }
    }
}
