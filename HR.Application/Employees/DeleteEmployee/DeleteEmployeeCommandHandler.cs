using HR.Domain;
using HR.Domain.Employees;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Employees.DeleteEmployee
{
    internal sealed class DeleteEmployeeCommandHandler : ICommandHandler<DeleteEmployeeCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public DeleteEmployeeCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (employee is null)
                return Result.Failure(EmployeeErrors.NotFound);

            var result = employee.Delete(); // بيتحقق من IsActive جوا الـ Entity
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.SaveChangesAsync(cancellationToken); // الـ Override هيعمل Soft Delete

            return Result.Success();
        }
    }
}
