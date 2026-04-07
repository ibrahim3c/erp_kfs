using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    public class ServiceTerminationRequest : Entity
    {
        public Guid EmployeeId { get; set; }
        public Guid ServiceTerminationTypeId { get; set; }

        public string RequestNumber { get; set; }
        public string IssuedTo { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime? RequestStartDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string FilePath { get; set; }

        // Audit Fields
        //public int? CreatedBy { get; set; }
        //public int? UpdatedBy { get; set; }
        //public int? DeletedBy { get; set; }

        // Navigation
        public Employee Employee { get; set; }
        public ServiceTerminationType ServiceTerminationType { get; set; }

        private ServiceTerminationRequest() { }
        public service
    }
}
