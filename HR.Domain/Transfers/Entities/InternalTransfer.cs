using HR.Domain.Transfers.Enums;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Transfers.Entities
{
    public class InternalTransfer : Entity
    {
        public Guid EmployeeId { get; private set; }
        public Guid FromDepartmentId { get; private set; }
        public Guid ToDepartmentId { get; private set; }
        public string Reason { get; private set; }
        public DateTime ExecutionDate { get; private set; }
        public Guid? NewJobTitleId { get; private set; }
        public string? AttachmentPath { get; private set; }
        public InternalTransferStatus Status { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }

        private InternalTransfer() { } // EF

        private InternalTransfer(Guid id, Guid employeeId, Guid fromDepartmentId, Guid toDepartmentId,
            string reason, DateTime executionDate, Guid? newJobTitleId, string? attachmentPath) : base(id)
        {
            
            EmployeeId = employeeId;
            FromDepartmentId = fromDepartmentId;
            ToDepartmentId = toDepartmentId;
            Reason = reason;
            ExecutionDate = executionDate;
            NewJobTitleId = newJobTitleId;
            AttachmentPath = attachmentPath;
            Status = InternalTransferStatus.PendingApproval;
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public static Result<InternalTransfer> Create(
            Guid employeeId, Guid fromDepartmentId, Guid toDepartmentId, string reason,
            DateTime executionDate, Guid? newJobTitleId, string? attachmentPath)
        {
            if (fromDepartmentId == toDepartmentId)
                return Result<InternalTransfer>.Failure(TranseferErrors.SameDepartment);

            if (string.IsNullOrWhiteSpace(reason))
                return Result<InternalTransfer>.Failure(TranseferErrors.ReasonRequired);

            return Result<InternalTransfer>.Success(new InternalTransfer(
                Guid.NewGuid(), employeeId, fromDepartmentId, toDepartmentId, reason, executionDate, newJobTitleId, attachmentPath));
        }

        public Result Approve()
        {
            if (Status != InternalTransferStatus.PendingApproval)
                return Result.Failure(TranseferErrors.InvalidStatus);

            Status = InternalTransferStatus.Approved;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
