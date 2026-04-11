using Modules.Shared.Domain;

namespace Geography.Domain
{
    public class Governorate : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        // Navigation Property
        public ICollection<CityCenter> CityCenters { get; set; }
    }
}
