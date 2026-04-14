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
            {
                return Result.Failure(EmployeeErrors.NotFound);
            }
            _unitOfWork.EmployeeRepository.Delete(employee);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
