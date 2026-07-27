using FluentValidation;

namespace HR.Application.Funds.CreateFundSubscription
{
    internal sealed class CreateFundSubscriptionValidator
        : AbstractValidator<CreateFundSubscriptionCommand>
    {
        public CreateFundSubscriptionValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار موظف");

            RuleFor(x => x.SubscriptionDate)
                .NotEmpty().WithMessage("تاريخ الاشتراك مطلوب");

            RuleFor(x => x.DeductionAmount)
                .GreaterThan(0).WithMessage("قيمة الخصم يجب أن تكون أكبر من صفر");

            RuleFor(x => x.BankAgreement)
                .Equal(true).WithMessage("يجب موافقة الموظف كتابياً على الخصم من الراتب");
        }
    }
}
