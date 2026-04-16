using HR.Domain;
using HR.Domain.Payrolls;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.CalculatePayrollCycle
{
    public sealed class CalculatePayrollCycleCommandHandler
        : ICommandHandler<CalculatePayrollCycleCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;
        private readonly IPayrollCalculationService _calculationService;

        public CalculatePayrollCycleCommandHandler(
            IHRUnitOfWork unitOfWork,
            IPayrollCalculationService calculationService)
        {
            _unitOfWork = unitOfWork;
            _calculationService = calculationService;
        }

        public async Task<Result<Guid>> Handle(
            CalculatePayrollCycleCommand request,
            CancellationToken cancellationToken)
        {
            // 1. إنشاء الدورة
            var cycleResult = PayrollCycle.Create(
                request.Month, request.Year, request.EmployeeCategory);

            if (cycleResult.IsFailure)
                return Result<Guid>.Failure(cycleResult.Error);

            var cycle = cycleResult.Value;

            // 1. حساب الرواتب
            var entries = await _calculationService.CalculateAsync(
                request.Month, 
                request.Year,
                request.EmployeeCategory,
                cycle!.Id,
                cancellationToken);

            foreach (var entry in entries)
                cycle.AddEntry(entry);

            cycle.MarkAsCalculated();

            // 2. خصم أقساط السلف فعلياً
            var activeLoans = await _unitOfWork.LoanRepository
                .GetActiveLoansByMonthAsync(request.Month, request.Year, cancellationToken);

            foreach (var loan in activeLoans)
                loan.PayNextInstallment(new DateTime(request.Year, request.Month, 1));

            // 3. خصم أقساط شراء المدد فعلياً
            var activePurchases = await _unitOfWork.InsurancePurchaseRepository
                .GetApprovedByMonthAsync(request.Month, request.Year, cancellationToken);

            foreach (var purchase in activePurchases)
                purchase.DeductMonthlyInstallment();

            // 4. حفظ كل حاجة دفعة واحدة
            _unitOfWork.PayrollRepository.AddPayrollCycle(cycle);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(cycle.Id);
        }
    }
}
