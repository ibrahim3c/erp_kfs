using Modules.Shared.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;
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

        public static Result<OrgUnitType> Create(string code, string name,int levelOrder, bool canHaveChild)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<OrgUnitType>.Failure(OrganizationErrors.CodeRequired);

            if (string.IsNullOrWhiteSpace(name))
                return Result<OrgUnitType>.Failure(OrganizationErrors.NameRequired);

            if (levelOrder < 0)
                return Result<OrgUnitType>.Failure(OrganizationErrors.InvalidLevelOrder);

            var orgUnitType = new OrgUnitType(
                Guid.NewGuid(),
                code,
                name,
                levelOrder,
                canHaveChild);

            return Result<OrgUnitType>.Success(orgUnitType);
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
