using FluentValidation;


namespace HR.Application.Payrolls.AddPayrollAdjustment
{
    internal sealed class AddPayrollAdjustmentValidator : AbstractValidator<AddPayrollAdjustmentCommand>
    {
        public AddPayrollAdjustmentValidator()
        {
            RuleFor(x => x.EntryId)
                .NotEmpty().WithMessage("يجب إدخال معرف السجل.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("يجب أن يكون المبلغ أكبر من صفر.");

                RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("يجب ألا يتجاوز سبب التعديل 500 حرف.");
        }
    }
}
