using Modules.Shared.Domain;
namespace HR.Domain.Employees
{
    public class EmployeeQualification : Entity
    {
        public Guid EmployeeId { get; private set; }
        public Guid QualificationTypeId { get; private set; }
        public string QualificationFullName { get; private set; }
        public string University { get; private set; }
        public bool IsVerified { get; private set; }

        private EmployeeQualification() { }

        internal EmployeeQualification(Guid employeeId, Guid typeId, string fullName, string university)
        {
            EmployeeId = employeeId;
            QualificationTypeId = typeId;
            QualificationFullName = fullName;
            University = university;
            IsVerified = false;
        }

        public void Verify()
        {
            IsVerified = true;
        }
    }
}
