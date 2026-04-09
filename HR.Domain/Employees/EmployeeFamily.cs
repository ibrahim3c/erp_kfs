using Modules.Shared.Domain;
namespace HR.Domain.Employees
{
    public class EmployeeFamily : Entity
    {
        public Guid EmployeeId { get; private set; }
        public string FullName { get; private set; }
        public string RelationshipType { get; private set; }
        public string HealthStatus { get; set; }
        public string NationalId { get; private set; }
        public string Phone { get; set; }
        public bool IsDisabled { get; private set; }

        private EmployeeFamily() { }

        public EmployeeFamily(Guid id,Guid employeeId, string fullName, string relationshipType, string healthStatus, string nationalId, string phone, bool isDisabled):base(id)
        {
            EmployeeId = employeeId;
            FullName = fullName;
            RelationshipType = relationshipType;
            HealthStatus = healthStatus;
            NationalId = nationalId;
            Phone = phone;
            IsDisabled = isDisabled;
        }

        public static Result<EmployeeFamily> Create(
                    Guid employeeId,
                    string fullName,
                    string relationshipType,
                    string healthStatus = null,
                    string nationalId = null,
                    string phone = null,
                    bool isDisabled = false)
        {
            if (employeeId == Guid.Empty)
                return Result<EmployeeFamily>.Failure(EmployeeErrors.EmployeeIdEmpty);

            if (string.IsNullOrWhiteSpace(fullName))
                return Result<EmployeeFamily>.Failure(EmployeeErrors.FullNameEmpty);

            if (string.IsNullOrWhiteSpace(relationshipType))
                return Result<EmployeeFamily>.Failure(EmployeeErrors.RelationshipTypeEmpty);

            // التحقق من الرقم القومي (في حال تم إدخاله، يجب أن يكون 14 رقماً)
            if (!string.IsNullOrWhiteSpace(nationalId) && nationalId.Length != 14)
                return Result<EmployeeFamily>.Failure(EmployeeErrors.InvalidNationalId);

            var familyMember = new EmployeeFamily(
                id: Guid.NewGuid(),
                employeeId: employeeId,
                fullName: fullName,
                relationshipType: relationshipType,
                healthStatus: healthStatus,
                nationalId: nationalId,
                phone: phone,
                isDisabled: isDisabled
            );

            return Result<EmployeeFamily>.Success(familyMember);
        }

        // --- Business Behaviors ---

        public Result MarkAsDisabled()
        {
            if (IsDisabled)
                return Result.Failure(EmployeeErrors.AlreadyDisabled);

            IsDisabled = true;
            return Result.Success();
        }

        public void UpdateHealthStatus(string newHealthStatus)
        {
            HealthStatus = newHealthStatus;
        }

        public void UpdateContactInfo(string phone)
        {
            Phone = phone;
        }
    }
}
