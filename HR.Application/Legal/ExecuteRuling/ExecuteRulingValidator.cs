using FluentValidation;

namespace HR.Application.Legal.ExecuteRuling
{
    internal sealed class ExecuteRulingValidator : AbstractValidator<ExecuteRulingCommand>
    {
        public ExecuteRulingValidator()
        {
            RuleFor(x => x.RulingId)
                .NotEmpty().WithMessage("معرف الحكم القضائي مطلوب");

            RuleFor(x => x.DecisionId)
                .NotEmpty().WithMessage("رقم القرار التنفيذي مطلوب");
        }
    }
}
