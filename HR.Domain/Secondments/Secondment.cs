using HR.Domain.Secondments.Enums;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Secondments
{
    public class Secondment : Entity
    {
        public Guid EmployeeId { get; private set; }
        public SecondmentType Type { get; private set; }
        public string HostEntityName { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public SalaryBearer SalaryBearer { get; private set; }
        public IncentiveBearer IncentiveBearer { get; private set; }
        public string FilePath { get; private set; }
        public bool ClearanceCompleted { get; private set; }
        public SecondmentStatus Status { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }

        private Secondment() { } // EF

        private Secondment(Guid id, Guid employeeId, SecondmentType type, string hostEntityName,
            DateTime startDate, DateTime endDate, SalaryBearer salaryBearer, IncentiveBearer incentiveBearer, string filePath) : base(id)
        {
            EmployeeId = employeeId;
            Type = type;
            HostEntityName = hostEntityName;
            StartDate = startDate;
            EndDate = endDate;
            SalaryBearer = salaryBearer;
            IncentiveBearer = incentiveBearer;
            FilePath = filePath;
            ClearanceCompleted = false;
            Status = SecondmentStatus.Active;
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public static Result<Secondment> Create(
            Guid employeeId, SecondmentType type, string hostEntityName,
            DateTime startDate, DateTime endDate, SalaryBearer salaryBearer, IncentiveBearer incentiveBearer, string filePath)
        {
            if (string.IsNullOrWhiteSpace(hostEntityName))
                return Result<Secondment>.Failure(SecondmentErrors.InvalidHostEntity);

            if (endDate <= startDate)
                return Result<Secondment>.Failure(SecondmentErrors.InvalidDate);

            // إعارة خارجية دائمًا بدون أجر من جهتنا (قاعدة عمل مذكورة في التصميم)
            if (type == SecondmentType.External && salaryBearer == SalaryBearer.OriginalEntity)
                return Result<Secondment>.Failure(SecondmentErrors.InvalidSalaryBearer);

            return Result<Secondment>.Success(new Secondment(
                Guid.NewGuid(), employeeId, type, hostEntityName, startDate, endDate, salaryBearer, incentiveBearer, filePath));
        }

        public Result Renew(DateTime newEndDate)
        {
            if (Status != SecondmentStatus.Active && Status != SecondmentStatus.PendingRenewal)
                return Result.Failure(SecondmentErrors.CannotRenew);

            if (newEndDate <= EndDate)
                return Result.Failure(SecondmentErrors.InvalidRenewalDate);

            EndDate = newEndDate;
            Status = SecondmentStatus.Active;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }

        public Result MarkClearanceCompleted()
        {
            ClearanceCompleted = true;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }

        public Result End()
        {
            if (Status == SecondmentStatus.Ended)
                return Result.Failure(SecondmentErrors.AlreadyEnded);

            if (!ClearanceCompleted)
                return Result.Failure(SecondmentErrors.ClearanceRequired);

            Status = SecondmentStatus.Ended;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
