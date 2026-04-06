using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    public class Employee : Entity
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string NationalId { get; private set; }
        public string Phone { get; private set; }
        public DateTime HireDate { get; private set; }
        public DateTime? TerminationDate { get; private set; }
        public bool IsActive { get; private set; }

        // Data related to Work setup
        public EmploymentType EmploymentType { get; private set; }
        public int? JobTitleId { get; private set; }
        public int? OrgUnitId { get; private set; }

        // Encapsulated Collections (Children Entities)
        private readonly List<EmployeeFamily> _families = new();
        public IReadOnlyCollection<EmployeeFamily> Families => _families.AsReadOnly();

        private readonly List<EmployeeQualification> _qualifications = new();
        public IReadOnlyCollection<EmployeeQualification> Qualifications => _qualifications.AsReadOnly();

        private readonly List<EmployeeDecision> _decisions = new();
        public IReadOnlyCollection<EmployeeDecision> Decisions => _decisions.AsReadOnly();
        private readonly List<LeadershipPositionHistory> _leadershipHistory = new();
        public IReadOnlyCollection<LeadershipPositionHistory> LeadershipHistory => _leadershipHistory.AsReadOnly();

        private readonly List<AcademicIncentiveRequest> _academicIncentiveRequests = new();
        public IReadOnlyCollection<AcademicIncentiveRequest> AcademicIncentiveRequests => _academicIncentiveRequests.AsReadOnly();

        private Employee() { } // EF Core Parameterless constructor

        // Constructor for Creation
        public Employee(string code, string name, string nationalId, DateTime hireDate, EmploymentType employmentType)
        {
            Code = code;
            Name = name;
            NationalId = nationalId;
            HireDate = hireDate;
            EmploymentType = employmentType;
            IsActive = true;
        }

        // --- Business Behaviors (Methods) ---

        public void AssignToPosition(int orgUnitId, int jobTitleId)
        {
            OrgUnitId = orgUnitId;
            JobTitleId = jobTitleId;
        }

        public void Terminate(DateTime terminationDate)
        {
            if (!IsActive) throw new InvalidOperationException("الموظف غير نشط بالفعل.");

            IsActive = false;
            TerminationDate = terminationDate;
        }

        // Managing Children Entities through the Aggregate Root
        public void AddFamilyMember(string fullName, string relationshipType, string nationalId)
        {
            var member = new EmployeeFamily(Id, fullName, relationshipType, nationalId);
            _families.Add(member);
        }

        public void AddQualification(Guid qualificationTypeId, string fullName, string university)
        {
            var qualification = new EmployeeQualification(Id, qualificationTypeId, fullName, university);
            _qualifications.Add(qualification);
        }
        public void AssignLeadershipPosition(Guid positionId, DateTime startDate, string decisionNumber, DateTime? decisionDate, string notes = "")
        {
            var historyRecord = new LeadershipPositionHistory(Id, positionId, startDate, decisionNumber, decisionDate, notes);
            _leadershipHistory.Add(historyRecord);
        }

        // السلوك الخاص بإضافة طلب جديد للموظف
        public void AddAcademicIncentiveRequest(Guid typeId, Guid qualificationId, string filePath, string notes = "")
        {
            var request = new AcademicIncentiveRequest(Id, typeId, qualificationId, filePath, notes);
            _academicIncentiveRequests.Add(request);
        }
    }
}
