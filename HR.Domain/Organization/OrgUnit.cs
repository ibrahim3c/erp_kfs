using Modules.Shared.Domain;

namespace HR.Domain.Organization
{
    public class OrgUnit : Entity
    {
        public Guid? ParentId { get; private set; }
        public Guid OrgUnitTypeId { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public bool IsActive { get; private set; }
        public Guid? GovernorateId { get; private set; }

        // Navigation Properties (Encapsulated)
        private readonly List<OrgUnit> _children = new();
        public IReadOnlyCollection<OrgUnit> Children => _children.AsReadOnly();
        public OrgUnit Parent { get; private set; }
        public OrgUnitType OrgUnitType { get; private set; }

        private OrgUnit() { } // For EF Core

        private OrgUnit(Guid id,string name, string code, Guid orgUnitTypeId, Guid? parentId, Guid? governorateId):base(id)
        {
            Name = name;
            Code = code;
            OrgUnitTypeId = orgUnitTypeId;
            ParentId = parentId;
            GovernorateId = governorateId;
            IsActive = true; // Default behavior
        }

        public static Result<OrgUnit> Create(string name, string code, Guid orgUnitTypeId, Guid? parentId = null, Guid? governorateId = null)
        {
            // Domain Validations
            if (string.IsNullOrWhiteSpace(name))
                return Result<OrgUnit>.Failure(OrgUnitErrors.NameEmpty);

            if (name.Length > 150)
                return Result<OrgUnit>.Failure(OrgUnitErrors.NameTooLong);

            if (string.IsNullOrWhiteSpace(code))
                return Result<OrgUnit>.Failure(OrgUnitErrors.CodeEmpty);

            if (code.Length > 50)
                return Result<OrgUnit>.Failure(OrgUnitErrors.CodeTooLong);

            if (orgUnitTypeId == Guid.Empty)
                return Result<OrgUnit>.Failure(OrgUnitErrors.OrgUnitTypeRequired);

            var orgUnit = new OrgUnit(
                Guid.NewGuid(),
                name.Trim(),
                code.Trim(),
                orgUnitTypeId,
                parentId,
                governorateId
            );

            return Result<OrgUnit>.Success(orgUnit);
        }

        // Business Behaviors
        public void Deactivate() =>  IsActive = false;
        public void Activate() => IsActive = true;

        public void UpdateDetails(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
