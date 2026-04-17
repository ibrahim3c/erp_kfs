using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Payrolls
{
    /// <summary>
    /// تسوية يدوية على راتب موظف — إضافة مكافأة أو خصم غرامة
    /// </summary>
    public class PayrollAdjustment : Entity
    {
        private PayrollAdjustment() { }

        private PayrollAdjustment(Guid id, Guid entryId,
            AdjustmentType type, decimal amount, string reason) : base(id)
        {
            EntryId = entryId;
            Type = type;
            Amount = amount;
            Reason = reason;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid EntryId { get; private set; }
        public AdjustmentType Type { get; private set; }
        public decimal Amount { get; private set; }
        public string Reason { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public PayrollEntry Entry { get; private set; }

        public static Result<PayrollAdjustment> Create(
            Guid entryId, AdjustmentType type, decimal amount, string reason)
            => Result<PayrollAdjustment>.Success(new PayrollAdjustment(Guid.NewGuid(), entryId, type, amount, reason));
    }
}
