using CollegeControlSystem.Domain.Abstractions;
using HR.Domain;
using HR.Domain.Employees;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.CreateEmployee
{
    internal sealed class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateEmployeeCommandHandler(
            IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // 1. Generate unique employee code (Assuming this method exists in your repo)
            // Since the UI states "يتم إنشاؤه تلقائياً", we handle the logic here.
            string generatedCode = await _unitOfWork.EmployeeRepository.GetNextCodeAsync(cancellationToken);

            // 2. Create Domain Entity
            var result = Employee.Create(
                code: generatedCode,
                name: request.Name,
                nationalId: request.NationalId,
                hireDate: request.HireDate,
                phone: request.Phone,
                dateOfBirth: request.DateOfBirth,
                gender: request.Gender,
                email: request.Email,
                address: request.Address,
                maritalStatus: request.MaritalStatus,
                cityCenterId: request.CityCenterId,
                villageId: request.VillageId,
                //qualificationTypeId: request.QualificationTypeId,
                //specialization: request.Specialization,
                employmentTypeId: request.EmploymentTypeId,
                jobTitleId: request.JobTitleId,
                jobGradeId: request.JobGradeId,
                functionalGroupId: request.FunctionalGroupId,
                orgUnitId: request.OrgUnitId
            );

            // 3. Handle Domain Validation Failures
            if (result.IsFailure)
            {
                return Result<Guid>.Failure(result.Error);
            }

            var employee = result.Value;

            // 4. Persist
            _unitOfWork.EmployeeRepository.Add(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(employee.Id);
        }
    }
}
