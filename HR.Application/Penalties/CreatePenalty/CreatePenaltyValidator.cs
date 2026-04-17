using FluentValidation;
using HR.Domain.Penalties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.CreatePenalty
{
    internal sealed class CreatePenaltyValidator : AbstractValidator<CreatePenaltyCommand>
    {
        public CreatePenaltyValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.ViolationDate)
                .NotEmpty().WithMessage("تاريخ المخالفة مطلوب")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("تاريخ المخالفة لا يجوز أن يكون في المستقبل");

            RuleFor(x => x.DecisionReference)
                .NotEmpty().WithMessage("مرجعية القرار مطلوبة")
                .MaximumLength(100);

            RuleFor(x => x.DeductionDays)
                .GreaterThan(0).WithMessage("عدد أيام الخصم يجب أن يكون أكبر من صفر")
                .When(x => x.ActionType == PenaltyActionType.Deduct
                         || x.ActionType == PenaltyActionType.Hold);

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("أسباب الجزاء مطلوبة")
                .MaximumLength(1000);
        }
    }
}
