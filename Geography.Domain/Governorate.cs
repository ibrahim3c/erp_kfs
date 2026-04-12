using Modules.Shared.Domain;

namespace Geography.Domain
{
    public sealed class Governorate : Entity
    {
        private readonly List<CityCenter> _cityCenters = new();

        private Governorate() { }

        private Governorate(Guid id, string name, string code) : base(id)
        {
            Name = name;
            Code = code;
        }

        public string Name { get; private set; }

        public string Code { get; private set; }

        public IReadOnlyCollection<CityCenter> CityCenters =>
            _cityCenters.AsReadOnly();

        public static Result<Governorate> Create(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Governorate>.Failure(GeoErrors.NameEmpty);

            if (string.IsNullOrWhiteSpace(code))
                return Result<Governorate>.Failure(GeoErrors.CodeEmpty);

            return Result<Governorate>.Success(
                new Governorate(Guid.NewGuid(), name, code));
        }

        public Result UpdateDetails(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(GeoErrors.NameEmpty);
            if (string.IsNullOrWhiteSpace(code))
                return Result.Failure(GeoErrors.CodeEmpty);
            Name = name;
            Code = code;
            return Result.Success();

        }
    }
}
