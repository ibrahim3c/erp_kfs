using Modules.Shared.Domain;
using System.ComponentModel.DataAnnotations;
namespace HR.Domain.JobStructures
{
    public class JobGrade : Entity
    {
        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public int GradeLevel { get; set; }

        public string Description { get; set; }

        public int YearsNo { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
