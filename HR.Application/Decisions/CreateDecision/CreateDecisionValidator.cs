using FluentValidation;

namespace HR.Application.Decisions.CreateDecision
{
    internal sealed class CreateDecisionValidator : AbstractValidator<CreateDecisionCommand>
    {
        public CreateDecisionValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("رقم القرار مطلوب")
                .MaximumLength(100).WithMessage("رقم القرار لا يزيد عن 100 حرف");

            RuleFor(x => x.DecisionDate)
                .NotEmpty().WithMessage("تاريخ الصدور مطلوب");

            RuleFor(x => x.DecisionTypeId)
                .NotEmpty().WithMessage("نوع القرار مطلوب");

            RuleFor(x => x.DecisionAuthorityId)
                .NotEmpty().WithMessage("جهة إصدار القرار مطلوبة");

            RuleFor(x => x.ValidTo)
                .GreaterThanOrEqualTo(x => x.ValidFrom)
                .When(x => x.ValidFrom.HasValue && x.ValidTo.HasValue)
                .WithMessage("تاريخ الانتهاء يجب أن يكون بعد تاريخ البداية");

            RuleFor(x => x.Subject)
                .MaximumLength(500).WithMessage("موضوع القرار لا يزيد عن 500 حرف");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("ملاحظات القرار لا تزيد عن 2000 حرف");

            RuleFor(x => x.EmployeeIds)
                .NotEmpty().WithMessage("يجب اختيار موظف واحد على الأقل");
        }
    }
}
