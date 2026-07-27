using FluentValidation;

namespace HR.Application.Funds.CreateFundClaim
{
    internal sealed class CreateFundClaimValidator
        : AbstractValidator<CreateFundClaimCommand>
    {
        public CreateFundClaimValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار موظف");

            RuleFor(x => x.ClaimType)
                .IsInEnum().WithMessage("نوع المطالبة غير صحيح");

            RuleFor(x => x.EventDate)
                .NotEmpty().WithMessage("تاريخ الحدث مطلوب");
        }
    }
}
