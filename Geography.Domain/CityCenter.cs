using Modules.Shared.Domain;

namespace Geography.Domain
{
    public sealed class CityCenter : Entity
    {
        private readonly List<LocalUnit> _localUnits = new();
        private readonly List<Village> _villages = new();

        private CityCenter() { }

        private CityCenter(Guid id, Guid governorateId, string name, string type)
            : base(id)
        {
            GovernorateId = governorateId;
            Name = name;
            Type = type;
        }

        public Guid GovernorateId { get; private set; }

        public string Name { get; private set; }

        public string Type { get; private set; } // center | city

        public IReadOnlyCollection<LocalUnit> LocalUnits =>
            _localUnits.AsReadOnly();

        public IReadOnlyCollection<Village> Villages =>
            _villages.AsReadOnly();
         public Governorate Governorate { get; private set; }
        public static Result<CityCenter> Create(
            Guid governorateId,
            string name,
            string type)
        {
            if (governorateId == Guid.Empty)
                return Result<CityCenter>.Failure(GeoErrors.GovernorateIdEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<CityCenter>.Failure(GeoErrors.NameEmpty);

            return Result<CityCenter>.Success(
                new CityCenter(Guid.NewGuid(), governorateId, name, type));
        }

        public Result UpdateDetails(Guid governorateId, string name, string type)
        {
            if (governorateId == Guid.Empty)
                return Result.Failure(GeoErrors.GovernorateIdEmpty);
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(GeoErrors.NameEmpty);
            GovernorateId = governorateId;
            Name = name;
            Type = type;
            return Result.Success();
        }
    }
}
