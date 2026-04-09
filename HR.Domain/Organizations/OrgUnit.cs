using Modules.Shared.Domain;

namespace HR.Domain.Organizations
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
                throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code is required.", nameof(code));
            if (orgUnitTypeId == Guid.Empty)
                throw new ArgumentException("OrgUnitTypeId is required.", nameof(orgUnitTypeId));
            return Result<OrgUnit>.Success(new OrgUnit(Guid.NewGuid(), name, code, orgUnitTypeId, parentId, governorateId));
        }

        // Business Behaviors
        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void UpdateDetails(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
