using CollegeControlSystem.Domain.Abstractions;
using HR.Domain;
using HR.Domain.Employees;
using MediatR;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.RemoveLeadershipPosition
{
    public class RemoveLeadershipPositionCommandHandler : IRequestHandler<RemoveLeadershipPositionCommand, Result<bool>>
    {
        private readonly IHRUnitOfWork _uow;

        public RemoveLeadershipPositionCommandHandler(IHRUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<bool>> Handle(RemoveLeadershipPositionCommand request, CancellationToken cancellationToken)
        {
            // 1. البحث عن الموظف
            var employee = await _uow.EmployeeRepository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
            {
                return Result<bool>.Failure(EmployeeErrors.NotFound);
            }

            // 2. إزالة المنصب القيادي (تصفير الـ ID)
            employee.RemoveLeadershipPosition();


            await _uow.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
