using HR.Domain;
using HR.Domain.Employees;
using Modules.Shared.Application.Interfaces;
using Modules.Shared.Application.IService;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Employees.CreateEmployee
{

    public sealed class CreateEmployeeCommandHandler
        : ICommandHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IIdentityService _identityService;

        public CreateEmployeeCommandHandler(
            IHRUnitOfWork unitOfWork,
            IFileService fileService,
            IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _identityService = identityService;
        }

        public async Task<Result<Guid>> Handle(
            CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            // 1. التحقق من تكرار الرقم القومي
            bool nationalIdExists = await _unitOfWork.EmployeeRepository
                .ExistsByNationalIdAsync(request.NationalId, cancellationToken);

            if (nationalIdExists)
                return Result<Guid>.Failure(EmployeeErrors.NationalIdAlreadyExists);

            // 2. توليد الكود التسلسلي
            string code = await _unitOfWork.EmployeeRepository.GetNextCodeAsync(cancellationToken);

            // 3. إنشاء الـ Employee aggregate
            string fullName = $"{request.FirstName} {request.FatherName} {request.LastName}".Trim();

            var employeeResult = Employee.Create(
                code: code,
                name: fullName,
                nationalId: request.NationalId,
                hireDate: request.HireDate,
                phone: request.Phone,
                dateOfBirth: request.DateOfBirth,
                gender: request.Gender,
                email: request.Email,
                address: request.Address,
                maritalStatus: request.MaritalStatus,
                isDisabled: request.IsDisabled,
                employmentTypeId: request.EmploymentTypeId,
                jobTitleId: request.JobTitleId,
                jobGradeId: request.JobGradeId,
                functionalGroupId: request.FunctionalGroupId,
                orgUnitId: request.OrgUnitId);

            if (employeeResult.IsFailure)
                return Result<Guid>.Failure(employeeResult.Error);

            var employee = employeeResult.Value;

            // 4. Upload documents via FileService then add E-File
            string? profileImagePath = null;
            string? nationalIdCardFrontPath = null;
            string? nationalIdCardBackPath = null;
            string? qualificationFilePath = null;
            string? birthCertificatePath = null;
            string? militaryFilePath = null;
            string? contractFilePath = null;
            string? policeClearancePath = null;
            string? marriageDocumentPath = null;

            if (request.ProfileImage != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.ProfileImage, "employees/profiles");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                profileImagePath = uploadResult.Value;
            }

            if (request.NationalIdCardFront != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.NationalIdCardFront, "employees/national-ids-front");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                nationalIdCardFrontPath = uploadResult.Value;
            }

            if (request.NationalIdCardBack != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.NationalIdCardBack, "employees/national-ids-back");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                nationalIdCardBackPath = uploadResult.Value;
            }

            if (request.QualificationFile != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.QualificationFile, "employees/qualifications");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                qualificationFilePath = uploadResult.Value;
            }

            if (request.BirthCertificate != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.BirthCertificate, "employees/birth-certificates");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                birthCertificatePath = uploadResult.Value;
            }

            if (request.MilitaryFile != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.MilitaryFile, "employees/military");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                militaryFilePath = uploadResult.Value;
            }

            if (request.ContractFile != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.ContractFile, "employees/contracts");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                contractFilePath = uploadResult.Value;
            }

            if (request.PoliceClearance != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.PoliceClearance, "employees/police-clearance");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                policeClearancePath = uploadResult.Value;
            }

            if (request.MarriageDocument != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(request.MarriageDocument, "employees/marriage");
                if (uploadResult.IsFailure)
                    return Result<Guid>.Failure(uploadResult.Error);
                marriageDocumentPath = uploadResult.Value;
            }

            bool hasAnyFile =
                !string.IsNullOrWhiteSpace(nationalIdCardFrontPath) ||
                !string.IsNullOrWhiteSpace(nationalIdCardBackPath) ||
                !string.IsNullOrWhiteSpace(qualificationFilePath) ||
                !string.IsNullOrWhiteSpace(birthCertificatePath) ||
                !string.IsNullOrWhiteSpace(militaryFilePath) ||
                !string.IsNullOrWhiteSpace(contractFilePath) ||
                !string.IsNullOrWhiteSpace(policeClearancePath) ||
                !string.IsNullOrWhiteSpace(marriageDocumentPath) ||
                !string.IsNullOrWhiteSpace(profileImagePath);

            if (hasAnyFile)
            {
                var fileResult = EmployeeFile.Create(
                    employeeId: employee.Id,
                    militaryFile: militaryFilePath,
                    qualificationFile: qualificationFilePath,
                    birthCertificateFile: birthCertificatePath,
                    policeClearanceCertificate: policeClearancePath,
                    nationalIdCardFront: nationalIdCardFrontPath,
                    nationalIdCardBack: nationalIdCardBackPath,
                    marriageDocument: marriageDocumentPath,
                    personalPhoto: profileImagePath,
                    contractFile: contractFilePath);

                if (fileResult.IsFailure)
                    return Result<Guid>.Failure(fileResult.Error);

                employee.AddEmployeeFile(fileResult.Value);
            }

            // 5. إضافة البيانات المالية — اختياري
            bool hasFinancialData =
                request.GrossSalary.HasValue ||
                request.BasicSalary2019.HasValue ||
                !string.IsNullOrWhiteSpace(request.InsuranceNumber) ||
                !string.IsNullOrWhiteSpace(request.BankName) ||
                !string.IsNullOrWhiteSpace(request.BankAccountNumber);

            if (hasFinancialData)
            {
                var financialResult = employee.AddFinancialInformation(
                    basicSalary2019: request.BasicSalary2019,
                    grossSalary: request.GrossSalary,
                    insuranceNumber: request.InsuranceNumber,
                    bankName: request.BankName,
                    bankAccount: request.BankAccountNumber,
                    hasFellowshipFund: request.HasFellowshipFund,
                    hasMedicalFund: request.HasMedicalFund);

                if (financialResult.IsFailure)
                    return Result<Guid>.Failure(financialResult.Error);
            }

            employee.AddEmployeeQualification(request.QualificationTypeId,
                request.QualificationFullName,
                request.Specialization,
                request.University,
                request.GraduationYear,
                request.Grade,
                qualificationFilePath,
                request.QualificationValidFrom,
                request.QualificationValidTo,
                request.QualificationNotes);

            // 6. Persist
            await _unitOfWork.EmployeeRepository.AddAsync(employee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Create system user for employee
            var createUserResult = await _identityService.CreateUserForEmployeeAsync(
                fullName: fullName,
                nationalId: request.NationalId,
                email: request.Email);

            if (createUserResult.IsSuccess)
            {
                employee.LinkUserId(createUserResult.Value);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Result<Guid>.Success(employee.Id);
        }
    }
}
