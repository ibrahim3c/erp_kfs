using System.ComponentModel.DataAnnotations;

namespace MyERP.Web.Models
{
    public class LeadershipAssignment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required] public string? PositionId { get; set; }
        [Required] public string? EmployeeId { get; set; }
        [Required] public DateTime AssignedDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        [Required] public bool IsCurrent { get; set; } = true;

        public virtual GlobalLeadershipPosition Position { get; set; } = null!;
        public virtual EmployeeAdmin Employee { get; set; } = null!;
            public string? HijriDate { get; set; } // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←

    }
}