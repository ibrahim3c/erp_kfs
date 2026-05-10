using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Snapshots
{
    public record EmployeeSnapshot
    {
        // ── من جدول Employees ────────────────────────────────
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Department { get; init; } = string.Empty;  // OrgUnitName

        // ── من جدول JobGrades ────────────────────────────────
        public Guid GradeId { get; init; }
        public string GradeCode { get; init; } = string.Empty;
        public string GradeName { get; init; } = string.Empty;
        public int GradeLevel { get; init; }
        public int GradeYearsNo { get; init; }

        // ── من PromotionHistory أو HireDate ──────────────────
        public DateTime GradeStartDate { get; init; }

        // ── Helper — بيبني JobGradeSnapshot للـ Domain ───────
        public JobGradeSnapshot ToGradeSnapshot()
            => new(GradeId, GradeCode, GradeName, GradeLevel, GradeYearsNo);
    }
}
