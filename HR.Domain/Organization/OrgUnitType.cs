using Modules.Shared.Domain;
namespace HR.Domain.Organization
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
                return Result<OrgUnitType>.Failure(OrgUnitTypeErrors.CodeEmpty);

            if (code.Length > 50)
                return Result<OrgUnitType>.Failure(OrgUnitTypeErrors.CodeTooLong);

            if (string.IsNullOrWhiteSpace(name))
                return Result<OrgUnitType>.Failure(OrgUnitTypeErrors.NameEmpty);

            if (name.Length > 100)
                return Result<OrgUnitType>.Failure(OrgUnitTypeErrors.NameTooLong);

            if (levelOrder < 0)
                return Result<OrgUnitType>.Failure(OrgUnitTypeErrors.LevelOrderInvalid);

            var orgUnitType = new OrgUnitType(
                Guid.NewGuid(),
                code.Trim(),
                name.Trim(),
                levelOrder,
                canHaveChild
            );

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
