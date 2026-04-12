using Modules.Shared.Domain;

namespace Geography.Domain
{
    public sealed class Village : Entity
    {
        private Village() { }

        private Village(Guid id, Guid localUnitId, string name)
            : base(id)
        {
            LocalUnitId = localUnitId;
            Name = name;
        }

        public Guid LocalUnitId { get; private set; }

        public string Name { get; private set; }
        public LocalUnit LocalUnit { get; private set; } = null!;

        public static Result<Village> Create(Guid localUnitId, string name)
        {
            if (localUnitId == Guid.Empty)
                return Result<Village>.Failure(GeoErrors.LocalUnitIdEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<Village>.Failure(GeoErrors.NameEmpty);

            return Result<Village>.Success(
                new Village(Guid.NewGuid(), localUnitId, name));
        }

        public Result UpdateDetails(Guid localUnitId, string name)
        {
            if (localUnitId == Guid.Empty)
                return Result.Failure(GeoErrors.LocalUnitIdEmpty);
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(GeoErrors.NameEmpty);
            LocalUnitId = localUnitId;
            Name = name;
            return Result.Success();
        }
    }
}
