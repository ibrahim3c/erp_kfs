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

namespace HR.Application.Employees.AssignLeadershipPosition
{
    public class AssignLeadershipPositionCommandHandler : IRequestHandler<AssignLeadershipPositionCommand, Result<bool>>
    {
        private readonly IHRUnitOfWork _uow; // بافتراض إنك بتستخدم UnitOfWork

        public AssignLeadershipPositionCommandHandler(IHRUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<bool>> Handle(AssignLeadershipPositionCommand request, CancellationToken cancellationToken)
        {
            // 1. البحث عن الموظف في قاعدة البيانات
            var employee = await _uow.EmployeeRepository.GetByIdAsync(request.EmployeeId);

            if (employee == null)      
                return Result<bool>.Failure(EmployeeErrors.NotFound);
            
            // 2. تحديث المنصب القيادي للموظف
             employee.AssignLeadershipPosition(request.LeadershipPositionId, request.Notes);


            // 3. تحديث وحفظ التغييرات
            _uow.EmployeeRepository.Update(employee);

            await _uow.SaveChangesAsync();


            // 4. إرجاع نتيجة النجاح
            return Result<bool>.Success(true);
        }
    }
}
