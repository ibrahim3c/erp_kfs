using HR.Domain.Decisions;
using HR.Domain.Employees.Events;
using HR.Domain.Employees.Qualifications;
using HR.Domain.Incentives;
using HR.Domain.Loans;
using HR.Domain.Permissions;
using HR.Domain.Terminations;
using Modules.Shared.Domain;
using Modules.Shared.Domain.Events;


namespace HR.Domain.Employees
{
    public class Employee : Entity
    {
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
        public bool IsDisabled { get; private set; }
        public Guid? CityCenterId { get; private set; }
        public Guid? VillageId { get; private set; }
        public Guid? OrgUnitId { get; private set; }

        //public Guid? QualificationTypeId { get; private set; }
        //public string Specialization { get; private set; }
        public Guid? EmploymentTypeId { get; private set; }
        public Guid? LeadershipPositionId { get; private set; }
        public Guid? JobTitleId { get; private set; }
        public Guid? JobGradeId { get; private set; }
        public Guid? FunctionalGroupId { get; private set; }

        public Guid? UserId { get; private set; }


        public EmployeeFinancial FinancialInfo { get; private set; }

        // --- Encapsulated Collections (Children Entities) ---
        private readonly List<EmployeeFamily> _employeeFamilies = new();
        public IReadOnlyCollection<EmployeeFamily> EmployeeFamilies => _employeeFamilies.AsReadOnly();

        private readonly List<EmployeeDecision> _employeeDecisions = new();
        public IReadOnlyCollection<EmployeeDecision> EmployeeDecisions => _employeeDecisions.AsReadOnly();

        private readonly List<ServiceTerminationRequest> _serviceTerminationRequests = new();
        public IReadOnlyCollection<ServiceTerminationRequest> ServiceTerminationRequests => _serviceTerminationRequests.AsReadOnly();

        private readonly List<AcademicIncentiveRequest> _academicIncentiveRequests = new();
        public IReadOnlyCollection<AcademicIncentiveRequest> AcademicIncentiveRequests => _academicIncentiveRequests.AsReadOnly();

        private readonly List<EmployeeFile> _employeeFiles = new();
        public IReadOnlyCollection<EmployeeFile> EmployeeFiles => _employeeFiles.AsReadOnly();

        private readonly List<EmployeeQualification> _employeeQualifications = new();
        public IReadOnlyCollection<EmployeeQualification> EmployeeQualifications => _employeeQualifications.AsReadOnly();

        // abdallah added here 
        private readonly List<Loan> _loans = new();
        public IReadOnlyCollection<Loan> Loans => _loans.AsReadOnly();
        private readonly List<InsurancePeriodPurchase> _insurancePeriodPurchases = new();
        public IReadOnlyCollection<InsurancePeriodPurchase> InsurancePeriodPurchases => _insurancePeriodPurchases.AsReadOnly();
        private readonly List<PermissionRequest> _permissionRequests = new();
        public IReadOnlyCollection<PermissionRequest> PermissionRequests => _permissionRequests.AsReadOnly();

        //private readonly List<Notification> _notificationsReceived = new();
        //public IReadOnlyCollection<Notification> NotificationsReceived => _notificationsReceived.AsReadOnly();

        //private readonly List<Notification> _notificationsSent = new();
        //public IReadOnlyCollection<Notification> NotificationsSent => _notificationsSent.AsReadOnly();

        // EF Core Parameterless constructor
        private Employee() { }
        private Employee(Guid id, string code, string name, string phone, string nationalId, DateTime? dateOfBirth, string gender, string email, string address, DateTime hireDate, DateTime? terminationDate, string maritalStatus, bool isActive, bool isDisabled, Guid? cityCenterId, Guid? villageId, Guid? employmentTypeId, Guid? jobTitleId, Guid? jobGradeId, Guid? functionalGroupId, Guid? orgUnitId, Guid? userId) : base(id)
        {
            Code = code;
            Name = name;
            Phone = phone;
            NationalId = nationalId;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Email = email;
            Address = address;
            HireDate = hireDate;
            TerminationDate = terminationDate;
            MaritalStatus = maritalStatus;
            IsActive = isActive;
            IsDisabled = isDisabled;
            CityCenterId = cityCenterId;
            VillageId = villageId;
            EmploymentTypeId = employmentTypeId;
            JobTitleId = jobTitleId;
            JobGradeId = jobGradeId;
            FunctionalGroupId = functionalGroupId;
            OrgUnitId = orgUnitId;
            UserId = userId;
        }

        public static Result<Employee> Create(
                    string code,
                    string name,
                    string nationalId,
                    DateTime hireDate,
                    string phone = null,
                    DateTime? dateOfBirth = null,
                    string gender = null,
                    string email = null,
                    string address = null,
                    string maritalStatus = null,
                    bool isDisabled=false,
                    Guid? cityCenterId = null,
                    Guid? villageId = null,
                    Guid? employmentTypeId = null,
                    Guid? jobTitleId = null,
                    Guid? jobGradeId = null,
                    Guid? functionalGroupId = null,
                    Guid? orgUnitId = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<Employee>.Failure(EmployeeErrors.CodeEmpty);

            if (string.IsNullOrWhiteSpace(name))
                return Result<Employee>.Failure(EmployeeErrors.NameEmpty);

            if (string.IsNullOrWhiteSpace(nationalId) || nationalId.Length != 14)
                return Result<Employee>.Failure(EmployeeErrors.InvalidNationalId);

            if (hireDate == default)
                return Result<Employee>.Failure(EmployeeErrors.InvalidHireDate);
            var employee = new Employee(
                id: Guid.NewGuid(),
                code: code,
                name: name,
                phone: phone,
                nationalId: nationalId,
                dateOfBirth: dateOfBirth,
                gender: gender,
                email: email,
                address: address,
                hireDate: hireDate,
                terminationDate: null,
                maritalStatus: maritalStatus,
                isActive: true,
                isDisabled: isDisabled,
                cityCenterId: cityCenterId,
                villageId: villageId,
                employmentTypeId: employmentTypeId,
                jobTitleId: jobTitleId,
                jobGradeId: jobGradeId,
                functionalGroupId: functionalGroupId,
                orgUnitId: orgUnitId,
                userId: null
            );

            return Result<Employee>.Success(employee);
        }
        // --- Business Behaviors (Methods) ---
        public Result Delete()
        {
            if (!IsActive)
                return Result.Failure(EmployeeErrors.AlreadyInactive);

            IsActive = false;
            return Result.Success();
        }
        public Result UpdateMainDetails(string name, string code, DateTime hireDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(EmployeeErrors.NameEmpty);

            if (string.IsNullOrWhiteSpace(code))
                return Result.Failure(EmployeeErrors.CodeEmpty);

            if (hireDate == default)
                return Result.Failure(EmployeeErrors.InvalidHireDate);

            Name = name;
            Code = code;
            HireDate = hireDate;


            // Optional: If status changes to inactive, you might want to automatically set TerminationDate
            // if (!isActive && TerminationDate == null) TerminationDate = DateTime.UtcNow;

            return Result.Success();
        }

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

        //public void UpdateQualification(Guid? qualificationTypeId, string specialization)
        //{
        //    QualificationTypeId = qualificationTypeId;
        //    Specialization = specialization;
        //}

        public void SetEmploymentType(Guid employmentTypeId)
        {
            EmploymentTypeId = employmentTypeId;
        }

        public Result Terminate(DateTime terminationDate)
        {

            if (!IsActive) return Result.Failure(EmployeeErrors.AlreadyInactive);

            IsActive = false;
            TerminationDate = terminationDate;
            return Result.Success();
        }
        // --- Managing Children Entities through the Aggregate Root ---

        public Result AddFamilyMember(
            string fullName,
            string relationshipType,
            string healthStatus = null,
            string nationalId = null,
            string phone = null,
            bool isDisabled = false)

        {
            var result = EmployeeFamily.Create(Id, fullName, relationshipType, healthStatus, nationalId, phone, isDisabled);
            if (result.IsFailure)
                Result.Failure(result.Error);

            _employeeFamilies.Add(result.Value);
            return Result.Success();
        }

        public Result RecordDecision(
            Guid decisionId,
            string description,
            DateTime? validFrom,
            DateTime? validTo,
            EmployeeDecisionStatus status,
            string notes)
        {
            var result = EmployeeDecision.Create(Id, decisionId, description, validFrom, validTo, status, notes);
            if (result.IsFailure)
                Result.Failure(result.Error);
            _employeeDecisions.Add(result.Value);
            return Result.Success();
        }

        public Result AddAcademicIncentiveRequest(
            Guid employeeId,
            Guid academicIncentiveTypeId,
            Guid qualificationId,
            DateTime requestDate,
            DateTime? requestAffectDate,
            string notes,
            string filePath)
        {
            var result = AcademicIncentiveRequest.Create(Id, academicIncentiveTypeId, qualificationId, requestDate, requestAffectDate, notes, filePath);
            if (result.IsFailure)
                Result.Failure(result.Error);
            _academicIncentiveRequests.Add(result.Value);
            return Result.Success();
        }

        public Result SubmitServiceTerminationRequest(
            Guid serviceTerminationTypeId,
            string requestNumber,
            string issuedTo,
            DateTime requestDate,
            DateTime? requestStartDate,
            string reason,
            string filePath)

        {
            var result = ServiceTerminationRequest.Create(Id, serviceTerminationTypeId, requestNumber, issuedTo, requestDate, requestStartDate, reason, filePath);
            if (result.IsFailure)
                return Result.Failure(result.Error);
            _serviceTerminationRequests.Add(result.Value);
            return Result.Success();
        }

    public Result AddFinancialInformation(
        decimal? basicSalary2019,
        decimal? grossSalary,
        string insuranceNumber,
        string bankName,
        string bankAccount,
        bool hasFellowshipFund,
        bool hasMedicalFund)
        {
            var financialResult = EmployeeFinancial.Create(
                Id, // نمرر الـ ID الخاص بالموظف الحالي
                basicSalary2019,
                grossSalary,
                insuranceNumber,
                bankName,
                bankAccount,
                hasFellowshipFund,
                hasMedicalFund);

            if (financialResult.IsFailure)
                return Result.Failure(financialResult.Error);

            FinancialInfo = financialResult.Value;
            return Result.Success();
        }
        public Result UpdateFinancialInformation(
                decimal? basicSalary2019,
                decimal? grossSalary,
                string? insuranceNumber,
                string? bankName,
                string? bankAccount,
                bool hasFellowshipFund,
                bool hasMedicalFund)
        {
            if (FinancialInfo is null)
                return AddFinancialInformation(
                    basicSalary2019, grossSalary,
                    insuranceNumber, bankName, bankAccount,
                    hasFellowshipFund, hasMedicalFund);

            FinancialInfo.Update(
                basicSalary2019, grossSalary,
                insuranceNumber, bankName, bankAccount,
                hasFellowshipFund, hasMedicalFund);

            return Result.Success();
        }
        // abdallah added here
        public Result AddEmployeeFile(EmployeeFile file)
        {
            if (file is null)
                return Result.Failure(new Error("Employee.InvalidFile", "الملف غير صحيح"));

            _employeeFiles.Add(file);
            return Result.Success();
        }
        public Result AssignLeadershipPosition(Guid positionId, string? notes)
        {
            if (positionId == Guid.Empty)
                return Result.Failure(new Error("Employee.InvalidPosition", "رقم المنصب غير صحيح."));

            LeadershipPositionId = positionId;

            // To separate the modules 
            // Raise domain event for leadership position assignment 
            RaiseDomainEvent(new LeadershipPositionAssignedDomainEvent(
                    EmployeeId: Id,
                    LeadershipPositionId: LeadershipPositionId ?? Guid.Empty,
                    AssignedAt: DateTime.UtcNow,
                    Notes: notes));

            return Result.Success();
        }

        public Result  RemoveLeadershipPosition()
        {
            RaiseDomainEvent(new LeadershipPositionRemovedDomainEvent(DateTime.UtcNow, Id));
            return Result.Success();
        }
        public void LinkUserId(Guid userId)
        {
            UserId = userId;
        }
        // add employee qualification 
        public Result AddEmployeeQualification(Guid qualificationTypeId,
            string qualificationFullName,
            string specialization = null,
            string university = null,
            int? graduationYear = null,
            string grade = null,
            string filePath = null,
            DateTime? validFrom = null,
            DateTime? validTo = null,
            string notes = null)

        {
            var result = EmployeeQualification.Create(Id, qualificationTypeId, qualificationFullName, specialization, university, graduationYear, grade, filePath, validFrom, validTo, notes);
            if (result.IsFailure)
                return Result.Failure(result.Error);
            _employeeQualifications.Add(result.Value);
            return Result.Success();
        }
    }

}