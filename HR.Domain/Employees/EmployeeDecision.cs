using Modules.Shared.Domain;
namespace HR.Domain.Employees
{
    public class EmployeeDecision : Entity
    {
        public int EmployeeId { get; private set; }
        public int DecisionId { get; private set; } // Foreign Key to Settings/Lookups
        public string Description { get; private set; }
        public DateTime? ValidFrom { get; private set; }
        public bool IsActive { get; private set; }

        private EmployeeDecision() { }

        public EmployeeDecision(int employeeId, int decisionId, string description, DateTime validFrom)
        {
            EmployeeId = employeeId;
            DecisionId = decisionId;
            Description = description;
            ValidFrom = validFrom;
            IsActive = true;
        }

        public void CancelDecision()
        {
            IsActive = false;
        }
    }
}
