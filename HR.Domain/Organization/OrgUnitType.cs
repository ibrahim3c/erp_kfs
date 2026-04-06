using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // 1. Parameterless Constructor for EF Core
        private OrgUnitType() { }

        // 2. Public Constructor for Creation
        public OrgUnitType(string code, string name, int levelOrder, bool canHaveChild)
        {
            Code = code;
            Name = name;
            LevelOrder = levelOrder;
            CanHaveChild = canHaveChild;
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
