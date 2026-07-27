using Dapper;
using HR.Domain;
using HR.Domain.ServiceTerms.Entities;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Command.ApproveServiceTerm
{
    public class ApproveServiceTermHandler : ICommandHandler<ApproveServiceTermCommand>
    {
        private readonly IHRUnitOfWork _hrUnitOfWork;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public ApproveServiceTermHandler(IHRUnitOfWork hrUnitOfWork,ISqlConnectionFactory sqlConnectionFactory)
        {
            _hrUnitOfWork = hrUnitOfWork;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result> Handle(ApproveServiceTermCommand request, CancellationToken cancellationToken)
        {
            var record = await _hrUnitOfWork.ServiceTermRepository.GetByIdAsync(request.ServiceTermId, cancellationToken);
            if (record == null)
                return Result.Failure(ServiceTermErrors.NotFound);

            // جلب تاريخ التعيين الأصلي للموظف
            using var connection = _sqlConnectionFactory.CreateConnection();
            var appointmentDate = await connection.QuerySingleOrDefaultAsync<DateTime?>(
                "SELECT HireDate FROM HR.Employees WHERE Id = @EmployeeId", new { record.EmployeeId });

            if (appointmentDate is null)
                return Result.Failure(ServiceTermErrors.EmployeeNotFound);

            var result = record.Approve(appointmentDate.Value);
            if (result.IsFailure) return result;

            await _hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
