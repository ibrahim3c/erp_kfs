using CollegeControlSystem.Domain.Abstractions;
using HR.Domain;
using HR.Domain.Employees;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.UpdateEmployee
{
    internal sealed class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public UpdateEmployeeCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (employee is null)
                return Result.Failure(EmployeeErrors.NotFound);

            // Note: Since you use a Rich Domain Model, DO NOT set properties directly like employee.Name = request.Name
            employee.UpdateContactInformation(request.Phone, request.Email, employee.Address, employee.CityCenterId, employee.VillageId);

            // You might need to add a specific method to your Employee domain entity to handle Name, Code, HireDate, and IsActive updates if it doesn't exist yet.
            employee.UpdateMainDetails(request.Name, request.Code, request.HireDate, request.IsActive);

            _unitOfWork.EmployeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
