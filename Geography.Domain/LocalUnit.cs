using Modules.Shared.Domain;

namespace Geography.Domain
{
    public sealed class LocalUnit : Entity
    {
        private readonly List<Village> _villages = new();

        private LocalUnit() { }

        private LocalUnit(Guid id, Guid cityCenterId, string name)
            : base(id)
        {
            CityCenterId = cityCenterId;
            Name = name;
        }

        public Guid CityCenterId { get; private set; }

        public string Name { get; private set; }

        public IReadOnlyCollection<Village> Villages =>
            _villages.AsReadOnly();

        public CityCenter CityCenter { get; private set; }
        public static Result<LocalUnit> Create(Guid cityCenterId, string name)
        {
            if (cityCenterId == Guid.Empty)
                return Result<LocalUnit>.Failure(GeoErrors.CityCenterIdEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<LocalUnit>.Failure(GeoErrors.NameEmpty);

            return Result<LocalUnit>.Success(
                new LocalUnit(Guid.NewGuid(), cityCenterId, name));
        }

        public Result UpdateDetails(Guid cityCenterId, string name)
        {
            if (cityCenterId == Guid.Empty)
                return Result.Failure(GeoErrors.CityCenterIdEmpty);
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(GeoErrors.NameEmpty);
            CityCenterId = cityCenterId;
            Name = name;
            return Result.Success();
        }
    }
}
