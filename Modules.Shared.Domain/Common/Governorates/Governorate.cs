using Modules.Shared.Domain.Common.City_Center;
using System.ComponentModel.DataAnnotations;

namespace Modules.Shared.Domain.Common.Governorates
{
    public class Governorate: Entity
    {
     
        public string Name { get; set; }
        public string Code { get; set; }

        // Navigation Property (DDD)
        private readonly List<CityCenter> _cityCenters  = new();
        public IReadOnlyCollection<CityCenter> CityCenters => _cityCenters.AsReadOnly();

        // Parameterless constructor for EF Core
        private Governorate() { }
        private Governorate(Guid id,string name,string code) : base(id)
        {
            Name = name;
            Code = code;
        }

        // factory method
        public static Result<Governorate> Create(Guid id, string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Governorate>.Failure(GovernorateErrors.NameEmpty);

            if (string.IsNullOrWhiteSpace(code))
                return Result<Governorate>.Failure(GovernorateErrors.CodeEmpty);

            var governorate = new Governorate(id, name, code);
            return Result<Governorate>.Success(governorate);
        }
        public static Governorate Seed(string name, string code)
        {
            return new Governorate(Guid.NewGuid(), name, code);
        }
    }
}
