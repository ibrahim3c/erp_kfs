using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Snapshots
{
    public record JobGradeSnapshot(
    Guid Id,          // GUID من Organization
    string Code,        // GD01 ... GD12
    string Name,        // "الدرجة الثانية ب"
    int GradeLevel,  // 1 → 12
    int YearsNo      // سنوات الاستحقاق
    );
}
