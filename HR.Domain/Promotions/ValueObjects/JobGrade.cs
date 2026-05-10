using HR.Domain.Promotions.Snapshots;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.ValueObjects
{
    /// <summary>
    /// الدرجة الوظيفية — مبنية على بيانات Organization.JobGrades
    /// GradeLevel: 1 = أعلى درجة (GD01) ← 12 = أدنى درجة (GD12)
    /// YearsNo: سنوات الاستحقاق مختلفة لكل درجة (من الجدول)
    /// </summary>
    public sealed class JobGrade : IEquatable<JobGrade>  // الـ Entity بيتقارن بالـ Id.  /  لكن الـ Value Object بيتقارن بالقيم الداخلية.
    {
        public string Code { get; }   // GD01
        public string Name { get; }   // "الدرجة الثانية ب"
        public int Level { get; }   // 1 → 12
        public int YearsNo { get; }   // سنوات الاستحقاق

        private JobGrade(string code, string name, int level, int yearsNo)
        {
            Code = code;
            Name = name;
            Level = level;
            YearsNo = yearsNo;
        }

        // Factory — بياخد بيانات من JobGradeSnapshot
        public static JobGrade FromSnapshot(JobGradeSnapshot snapshot)
            => new(snapshot.Code, snapshot.Name,
                   snapshot.GradeLevel, snapshot.YearsNo);

        public bool HasNextGrade() => Level > 1;

        public Result<int> NextGradeLevel()
            => HasNextGrade()
                ? Result<int>.Success(Level - 1)
                : Result<int>.Failure(
                    new Error("JobGrade.MaxGrade", $"{Name} هي أعلى درجة"));

        public bool HasCompletedRequiredYears(decimal actualYears)
            => actualYears >= YearsNo;

        // Value Object — يتقارن بالقيمة مش بال اي دي 
        public bool Equals(JobGrade? other)
            => other is not null
            && Code == other.Code
            && Level == other.Level;

        public override bool Equals(object? obj) => Equals(obj as JobGrade);
        public override int GetHashCode() => HashCode.Combine(Code, Level); // لازم أي Object يعمل Override لـ Equals  ->  يعمل Override لـ GetHashCode.
       public override string ToString() => Name;
    }
}
