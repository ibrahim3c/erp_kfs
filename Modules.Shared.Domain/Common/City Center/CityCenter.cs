
using Modules.Shared.Domain.Common.Governorates;
using Modules.Shared.Domain.Common.Local_Unit;
using Modules.Shared.Domain.Common.Villages;

namespace Modules.Shared.Domain.Common.City_Center
{
    public class CityCenter : Entity
    {
   
        public int GovernorateId { get; set; }
        public string Name { get; set; }

        public CityCenterType Type { get; set; } // center | city

        // Navigation And Enscapulated
        public Governorate Governorate { get; set; }
        private readonly List<LocalUnit> _localUnits = new();
        public IReadOnlyCollection<LocalUnit> LocalUnits => _localUnits.AsReadOnly();

        private readonly List<Village> _villages = new();
        public IReadOnlyCollection<Village> Villages => _villages.AsReadOnly();

        // parameterless ctor
        private CityCenter() { }
        private CityCenter(Guid id, int governorateId, string name, CityCenterType type) : base(id)
        {
            GovernorateId = governorateId;
            Name = name;
            Type = type;
        }

        // factory method
        public static Result<CityCenter> Create(Guid id, int governorateId, string name, CityCenterType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<CityCenter>.Failure(CityCenterErrors.NameEmpty);

            if (!Enum.IsDefined(typeof(CityCenterType), type))
                return Result<CityCenter>.Failure(CityCenterErrors.InvalidType);

            if (name.Length > 100)
                return Result<CityCenter>.Failure(CityCenterErrors.NameTooLong);
            if (governorateId <= 0)
                return Result<CityCenter>.Failure(CityCenterErrors.GovernorateRequired);
            if (id == Guid.Empty)
                return Result<CityCenter>.Failure(CityCenterErrors.InvalidId);

            var cityCenter = new CityCenter(Guid.NewGuid(), governorateId, name, type);
            return Result<CityCenter>.Success(cityCenter);
        }

        public void ChangeName(string name) => Name = name;
    }
}
