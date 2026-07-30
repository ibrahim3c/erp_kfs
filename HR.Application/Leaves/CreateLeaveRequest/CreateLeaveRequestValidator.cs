using FluentValidation;

namespace HR.Application.Leaves.CreateLeaveRequest
{
    internal sealed class CreateLeaveRequestValidator
        : AbstractValidator<CreateLeaveRequestCommand>
    {
        public CreateLeaveRequestValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار موظف");

            RuleFor(x => x.LeaveCategory)
                .IsInEnum().WithMessage("نوع الأجازة غير صحيح");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("تاريخ البداية مطلوب");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("تاريخ النهاية مطلوب");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("تاريخ النهاية يجب أن يكون بعد أو يساوي تاريخ البداية");
        }
    }
}
