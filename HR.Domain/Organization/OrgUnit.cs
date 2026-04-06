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

        private OrgUnit() { } // For EF Core

        public OrgUnit(string name, string code, Guid orgUnitTypeId, Guid? parentId, Guid? governorateId)
        {
            Name = name;
            Code = code;
            OrgUnitTypeId = orgUnitTypeId;
            ParentId = parentId;
            GovernorateId = governorateId;
            IsActive = true; // Default behavior
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
