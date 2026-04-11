using Modules.Shared.Domain;
namespace Organization.Domain
{
    public class OrgUnitType : Entity // هيرث الـ Guid Id من هنا
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int LevelOrder { get; private set; }
        public bool CanHaveChild { get; private set; }

        // حماية الـ Collection الخاص بالإدارات المرتبطة بهذا النوع
        private readonly List<OrgUnit> _orgUnits = new();
        public IReadOnlyCollection<OrgUnit> OrgUnits => _orgUnits.AsReadOnly();

        private OrgUnitType() { }

        private OrgUnitType(Guid id,string code, string name, int levelOrder, bool canHaveChild):base(id)
        {
            Code = code;
            Name = name;
            LevelOrder = levelOrder;
            CanHaveChild = canHaveChild;
        }

        public static Result<OrgUnitType> Create(string code, string name, int levelOrder, bool canHaveChild)
        {
            // Domain Validations
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code is required.", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (levelOrder < 0)
                throw new ArgumentException("LevelOrder must be non-negative.", nameof(levelOrder));
            return Result<OrgUnitType>.Success(new OrgUnitType(Guid.NewGuid(), code, name, levelOrder, canHaveChild));
        }

        // 3. Business Behaviors (Methods)
        public void UpdateDetails(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public void UpdateHierarchyRules(int levelOrder, bool canHaveChild)
        {
            LevelOrder = levelOrder;
            CanHaveChild = canHaveChild;
        }
    }
}
