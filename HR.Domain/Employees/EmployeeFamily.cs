using Modules.Shared.Domain;
namespace HR.Domain.Employees
{
    public class EmployeeFamily : Entity
    {
        public Guid EmployeeId { get; private set; }
        public string FullName { get; private set; }
        public string RelationshipType { get; private set; }
        public string NationalId { get; private set; }
        public bool IsDisabled { get; private set; }

        private EmployeeFamily() { }

        // Internal constructor so only the Employee Aggregate can create it
        internal EmployeeFamily(Guid employeeId, string fullName, string relationshipType, string nationalId)
        {
            EmployeeId = employeeId;
            FullName = fullName;
            RelationshipType = relationshipType;
            NationalId = nationalId;
            IsDisabled = false;
        }

        public void MarkAsDisabled()
        {
            IsDisabled = true;
        }
    }
}
