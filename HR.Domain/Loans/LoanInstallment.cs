using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Loans
{
    public class LoanInstallment : Entity
    {
        private LoanInstallment() { }

        private LoanInstallment(Guid id, Guid loanId, decimal amount, DateTime dueDate) : base(id)
        {
            LoanId = loanId;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = false;
        }

        public Guid LoanId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime DueDate { get; private set; }
        public bool IsPaid { get; private set; }
        public DateTime? PaidAt { get; private set; }

        // Navigation
        public Loan Loan { get; private set; }

        // ─── Factory (internal — بس الـ Loan يقدر يستخدمها) ──────────
        public static Result<LoanInstallment> Create(Guid loanId, decimal amount, DateTime dueDate)
        {
            if (amount <= 0) return Result<LoanInstallment>.Failure(LoanErrors.InstallmentGreaterThanZero);

            var loan = new LoanInstallment(Guid.NewGuid(), loanId, amount, dueDate);
            return Result<LoanInstallment>.Success(loan);
        }

        public void MarkAsPaid(DateTime paymentDate)
        {
            if (IsPaid) return; // idempotent
            IsPaid = true;
            PaidAt = paymentDate;
        }
    }
}
