using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Modules.Shared.Domain.Common.City_Center;
using Modules.Shared.Domain.Common.Villages;

namespace Modules.Shared.Domain.Common.Local_Unit
{
    public class LocalUnit : Entity
    {
        public int CityCenterId { get; set; }
        public string Name { get; set; }

        // Navigation Properties
        public CityCenter CityCenter { get; set; }
        // Enscapulated (DDD)
        private readonly List<Village> _villages = new();
        public IReadOnlyCollection<Village> Villages => _villages.AsReadOnly();
    
    }
}
