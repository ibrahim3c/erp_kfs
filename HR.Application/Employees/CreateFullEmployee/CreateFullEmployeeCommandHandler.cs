using CollegeControlSystem.Domain.Abstractions;
using HR.Domain;
using HR.Domain.Employees;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.CreateFullEmployee
{

    public sealed class CreateFullEmployeeCommandHandler
        : ICommandHandler<CreateFullEmployeeCommand, Guid>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateFullEmployeeCommandHandler(
            IEmployeeRepository employeeRepository,
            IHRUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateFullEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            // 1. التحقق من تكرار الرقم القومي
            bool nationalIdExists = await _employeeRepository
                .ExistsByNationalIdAsync(request.NationalId, cancellationToken);

            if (nationalIdExists)
                return Result<Guid>.Failure(EmployeeErrors.NationalIdAlreadyExists);

            // 2. توليد الكود التسلسلي
            string code = await _employeeRepository.GetNextCodeAsync(cancellationToken);

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
                jobTitleId: request.JobTitleId,          // JobTitleName = free text — يُحل لاحقاً
                jobGradeId: request.JobGradeId,
                functionalGroupId: null,
                orgUnitId: request.OrgUnitId);

            if (employeeResult.IsFailure)
                return Result<Guid>.Failure(employeeResult.Error);

            var employee = employeeResult.Value;

            // 4. إضافة المستندات (E-File) — اختياري بالكامل
            bool hasAnyFile =
                !string.IsNullOrWhiteSpace(request.NationalIdCardPath) ||
                !string.IsNullOrWhiteSpace(request.QualificationFilePath) ||
                !string.IsNullOrWhiteSpace(request.BirthCertificatePath) ||
                !string.IsNullOrWhiteSpace(request.MilitaryFilePath) ||
                !string.IsNullOrWhiteSpace(request.ContractFilePath) ||
                !string.IsNullOrWhiteSpace(request.PoliceClearancePath) ||
                !string.IsNullOrWhiteSpace(request.ProfileImagePath);

            if (hasAnyFile)
            {
                var fileResult = EmployeeFile.Create(
                    employeeId: employee.Id,
                    militaryFile: request.MilitaryFilePath,
                    qualificationFile: request.QualificationFilePath,
                    birthCertificateFile: request.BirthCertificatePath,
                    policeClearanceCertificate: request.PoliceClearancePath,
                    nationalIdCardFront: request.NationalIdCardPath,
                    nationalIdCardBack: null,
                    marriageDocument: null,
                    personalPhoto: request.ProfileImagePath,
                    contractFile: request.ContractFilePath);

                if (fileResult.IsFailure)
                    return Result<Guid>.Failure(fileResult.Error);

                // EmployeeFile يُضاف عبر EF navigation أو repository مستقل
                // — حسب setup الـ DbContext عندك
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

            // 6. Persist
            await _employeeRepository.AddAsync(employee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(employee.Id);
        }
    }
}
