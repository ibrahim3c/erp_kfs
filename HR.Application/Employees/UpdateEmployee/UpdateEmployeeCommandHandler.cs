using HR.Domain;
using HR.Domain.Employees;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Application.IService;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.UpdateEmployee
{

    public sealed class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateEmployeeCommandHandler(IHRUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.Id, cancellationToken);
            if (employee is null)
                return Result.Failure(EmployeeErrors.NotFound);

            // 1. البيانات الأساسية
            var mainResult = employee.UpdateMainDetails(request.Name, request.Code, request.HireDate);
            if (!mainResult.IsSuccess) return mainResult;

            employee.UpdatePersonalDetails(request.DateOfBirth, request.Gender, request.MaritalStatus);

            employee.UpdateContactInformation(request.Phone, request.Email, request.Address,
                employee.CityCenterId, employee.VillageId);

            employee.AssignToPosition(request.OrgUnitId, request.JobTitleId,
                request.JobGradeId, request.FunctionalGroupId);

            if (request.EmploymentTypeId.HasValue)
                employee.SetEmploymentType(request.EmploymentTypeId.Value);

            // 2. البيانات المالية
            var financialResult = employee.UpdateFinancialInformation(
                request.BasicSalary2019, request.GrossSalary,
                request.InsuranceNumber, request.BankName, request.BankAccountNumber,
                request.HasFellowshipFund, request.HasMedicalFund);
            if (!financialResult.IsSuccess) return financialResult;

            // 3. رفع الملفات زي الـ Create
            async Task<string?> UploadIfExists(IFormFile? file, string folder, string? current)
            {
                if (file == null || file.Length == 0) return current; // فضّل القديم
                var result = await _fileService.UploadFileAsync(file, folder);
                return result.IsSuccess ? result.Value : current;
            }

            string? personalPhoto = await UploadIfExists(request.ProfileImage, "employees/profiles", request.CurrentPersonalPhoto);
            string? nationalIdFront = await UploadIfExists(request.NationalIdCardFront, "employees/national-ids-front", request.CurrentNationalIdCardFront);
            string? nationalIdBack = await UploadIfExists(request.NationalIdCardBack, "employees/national-ids-back", request.CurrentNationalIdCardBack);
            string? qualificationFile = await UploadIfExists(request.QualificationFile, "employees/qualifications", request.CurrentQualificationFile);
            string? birthCertificate = await UploadIfExists(request.BirthCertificate, "employees/birth-certificates", request.CurrentBirthCertificateFile);
            string? militaryFile = await UploadIfExists(request.MilitaryFile, "employees/military", request.CurrentMilitaryFile);
            string? contractFile = await UploadIfExists(request.ContractFile, "employees/contracts", request.CurrentContractFile);
            string? policeClearance = await UploadIfExists(request.PoliceClearance, "employees/police-clearance", request.CurrentPoliceClearance);
            string? marriageDocument = await UploadIfExists(request.MarriageDocument, "employees/marriage", request.CurrentMarriageDocument);

            // 4. حفظ الملفات
            var existingFile = employee.EmployeeFiles.FirstOrDefault();

            if (existingFile is not null)
            {
                existingFile.UpdateFiles(personalPhoto, nationalIdFront, nationalIdBack,
                    qualificationFile, birthCertificate, militaryFile,
                    contractFile, policeClearance, marriageDocument);
            }
            else
            {
                bool hasAnyFile =
                    !string.IsNullOrWhiteSpace(personalPhoto) ||
                    !string.IsNullOrWhiteSpace(nationalIdFront) ||
                    !string.IsNullOrWhiteSpace(nationalIdBack) ||
                    !string.IsNullOrWhiteSpace(qualificationFile) ||
                    !string.IsNullOrWhiteSpace(birthCertificate) ||
                    !string.IsNullOrWhiteSpace(militaryFile) ||
                    !string.IsNullOrWhiteSpace(contractFile) ||
                    !string.IsNullOrWhiteSpace(policeClearance) ||
                    !string.IsNullOrWhiteSpace(marriageDocument);

                if (hasAnyFile)
                {
                    var fileResult = EmployeeFile.Create(
                        employeeId: employee.Id,
                        personalPhoto: personalPhoto,
                        nationalIdCardFront: nationalIdFront,
                        nationalIdCardBack: nationalIdBack,
                        qualificationFile: qualificationFile,
                        birthCertificateFile: birthCertificate,
                        militaryFile: militaryFile,
                        contractFile: contractFile,
                        policeClearanceCertificate: policeClearance,
                        marriageDocument: marriageDocument);

                    if (!fileResult.IsSuccess) return fileResult;
                    employee.AddEmployeeFile(fileResult.Value);
                }
            }

            //_unitOfWork.EmployeeRepository.Update(employee);  because we are tracking the entity, we don't need to explicitly call Update
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

