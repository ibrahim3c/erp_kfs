using Modules.Shared.Domain;
namespace HR.Domain.Employees
{
    public class EmployeeFile : Entity
    {
        public int EmployeeId { get; private set; }
        public string MilitaryFile { get; private set; }
        public string QualificationFile { get; private set; }
        public string PersonalPhoto { get; private set; }

        private EmployeeFile() { }

        public EmployeeFile(int employeeId)
        {
            EmployeeId = employeeId;
        }

        public void UpdatePersonalPhoto(string newPhotoPath)
        {
            PersonalPhoto = newPhotoPath;
        }
    }
}
