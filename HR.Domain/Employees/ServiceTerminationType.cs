using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    public class ServiceTerminationType : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RequiresNoticePeriod { get; set; }
        public bool IsActive { get; set; } = true;

        // Audit Fields (Consider moving these to BaseEntity if they are repeated on all tables)
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }

        // Navigation
        public ICollection<ServiceTerminationRequest> ServiceTerminationRequests { get; set; }
            = new List<ServiceTerminationRequest>();
    }
}
