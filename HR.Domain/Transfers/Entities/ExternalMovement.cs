using HR.Domain.Transfers.Enums;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Transfers.Entities
{
    public class ExternalMovement : Entity
    {
        public Guid EmployeeId { get; private set; }
        public ExternalMovementType Type { get; private set; }
        public MovementDirection Direction { get; private set; }
        public string OtherEntityName { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public SalaryBearer? SalaryBearer { get; private set; }
        public string? AttachmentPath { get; private set; }
        public ExternalMovementStatus Status { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }

        private ExternalMovement() { } // EF

        private ExternalMovement(Guid id, Guid employeeId, ExternalMovementType type, MovementDirection direction,
            string otherEntityName, DateTime? startDate, DateTime? endDate, SalaryBearer? salaryBearer, string? attachmentPath) : base(id)
        {
           
            EmployeeId = employeeId;
            Type = type;
            Direction = direction;
            OtherEntityName = otherEntityName;
            StartDate = startDate;
            EndDate = endDate;
            SalaryBearer = salaryBearer;
            AttachmentPath = attachmentPath;
            Status = ExternalMovementStatus.Active;
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public static Result<ExternalMovement> Create(
            Guid employeeId, ExternalMovementType type, MovementDirection direction, string otherEntityName,
            DateTime? startDate, DateTime? endDate, SalaryBearer? salaryBearer, string? attachmentPath)
        {
            if (string.IsNullOrWhiteSpace(otherEntityName))
                return Result<ExternalMovement>.Failure(TranseferErrors.InvalidEntity);

            // الندب لازم يكون له تاريخ بداية ونهاية وجهة تتحمل الراتب
            if (type == ExternalMovementType.Secondment)
            {
                if (startDate is null || endDate is null)
                    return Result<ExternalMovement>.Failure(TranseferErrors.DatesRequired);

                if (endDate <= startDate)
                    return Result<ExternalMovement>.Failure(TranseferErrors.InvalidDates);

                if (salaryBearer is null)
                    return Result<ExternalMovement>.Failure(TranseferErrors.SalaryBearerRequired);
            }

            return Result<ExternalMovement>.Success(new ExternalMovement(
                Guid.NewGuid(), employeeId, type, direction, otherEntityName, startDate, endDate, salaryBearer, attachmentPath));
        }

        public Result Renew(DateTime newEndDate)
        {
            if (Type != ExternalMovementType.Secondment)
                return Result.Failure(TranseferErrors.NotSecondment);

            if (Status != ExternalMovementStatus.Active)
                return Result.Failure(TranseferErrors.NotActive);

            if (newEndDate <= EndDate)
                return Result.Failure(TranseferErrors.InvalidRenewalDate);

            EndDate = newEndDate;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }

        public Result End()
        {
            if (Status == ExternalMovementStatus.Ended)
                return Result.Failure(TranseferErrors.AlreadyEnded);

            Status = ExternalMovementStatus.Ended;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
