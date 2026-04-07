using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    public class Employee : Entity
    {
        // --- Properties with Private Setters ---
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string NationalId { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string Gender { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public DateTime HireDate { get; private set; }
        public DateTime? TerminationDate { get; private set; }
        public string MaritalStatus { get; private set; }
        public bool IsActive { get; private set; }

        // --- Work Setup & Categorization IDs ---
        public Guid? CityCenterId { get; private set; }
        public Guid? VillageId { get; private set; }
        public Guid? QualificationTypeId { get; private set; }
        public string Specialization { get; private set; }
        public Guid? EmploymentTypeId { get; private set; }
        public Guid? JobTitleId { get; private set; }
        public Guid? JobGradeId { get; private set; }
        public Guid? FunctionalGroupId { get; private set; }
        public Guid? OrgUnitId { get; private set; }

        // --- Encapsulated Collections (Children Entities) ---
        private readonly List<EmployeeFamily> _employeeFamilies = new();
        public IReadOnlyCollection<EmployeeFamily> EmployeeFamilies => _employeeFamilies.AsReadOnly();

        private readonly List<EmployeeDecision> _employeeDecisions = new();
        public IReadOnlyCollection<EmployeeDecision> EmployeeDecisions => _employeeDecisions.AsReadOnly();

        //private readonly List<ServiceTerminationRequest> _serviceTerminationRequests = new();
        //public IReadOnlyCollection<ServiceTerminationRequest> ServiceTerminationRequests => _serviceTerminationRequests.AsReadOnly();

        private readonly List<AcademicIncentiveRequest> _academicIncentiveRequests = new();
        public IReadOnlyCollection<AcademicIncentiveRequest> AcademicIncentiveRequests => _academicIncentiveRequests.AsReadOnly();

        //private readonly List<Notification> _notificationsReceived = new();
        //public IReadOnlyCollection<Notification> NotificationsReceived => _notificationsReceived.AsReadOnly();

        //private readonly List<Notification> _notificationsSent = new();
        //public IReadOnlyCollection<Notification> NotificationsSent => _notificationsSent.AsReadOnly();

        // EF Core Parameterless constructor
        private Employee() { }

        // Constructor for Creation
        public Employee(string code, string name, string nationalId, DateTime hireDate)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NationalId = nationalId;
            HireDate = hireDate;
            IsActive = true;
        }

        // --- Business Behaviors (Methods) ---

        public void UpdatePersonalDetails(DateTime? dateOfBirth, string gender, string maritalStatus)
        {
            DateOfBirth = dateOfBirth;
            Gender = gender;
            MaritalStatus = maritalStatus;
        }

        public void UpdateContactInformation(string phone, string email, string address, Guid? cityCenterId, Guid? villageId)
        {
            Phone = phone;
            Email = email;
            Address = address;
            CityCenterId = cityCenterId;
            VillageId = villageId;
        }

        public void AssignToPosition(Guid? orgUnitId, Guid? jobTitleId, Guid? jobGradeId, Guid? functionalGroupId)
        {
            OrgUnitId = orgUnitId;
            JobTitleId = jobTitleId;
            JobGradeId = jobGradeId;
            FunctionalGroupId = functionalGroupId;
        }

        public void UpdateQualification(Guid? qualificationTypeId, string specialization)
        {
            QualificationTypeId = qualificationTypeId;
            Specialization = specialization;
        }

        public void SetEmploymentType(Guid employmentTypeId)
        {
            EmploymentTypeId = employmentTypeId;
        }

        public void Terminate(DateTime terminationDate)
        {
            if (!IsActive) throw new InvalidOperationException("الموظف غير نشط بالفعل.");

            IsActive = false;
            TerminationDate = terminationDate;
        }

        // --- Managing Children Entities through the Aggregate Root ---

        public void AddFamilyMember(string fullName, string relationshipType, string nationalId)
        {
            // Assuming EmployeeFamily constructor matches these parameters and takes the EmployeeId
            var member = new EmployeeFamily(Id, fullName, relationshipType, nationalId);
            _employeeFamilies.Add(member);
        }

        public void RecordDecision(Guid decisionId, string description,DateTime validFrom)
        {
            var decision = new EmployeeDecision(Id, decisionId, description, validFrom);
            _employeeDecisions.Add(decision);
        }

        public void AddAcademicIncentiveRequest(Guid typeId, Guid qualificationId, string filePath, string notes = "")
        {
            var request = new AcademicIncentiveRequest(Id, typeId, qualificationId, filePath, notes);
            _academicIncentiveRequests.Add(request);
        }

        public void SubmitServiceTerminationRequest(DateTime requestedDate, string reason)
        {
            var request = new ServiceTerminationRequest(Id, requestedDate, reason);
            _serviceTerminationRequests.Add(request);
        }
    }
}