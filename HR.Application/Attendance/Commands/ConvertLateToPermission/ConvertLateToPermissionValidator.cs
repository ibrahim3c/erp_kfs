using FluentValidation;

namespace HR.Application.Attendance.Commands.ConvertLateToPermission
{
    internal sealed class ConvertLateToPermissionValidator
        : AbstractValidator<ConvertLateToPermissionCommand>
    {
        public ConvertLateToPermissionValidator()
        {
            RuleFor(x => x.AttendanceRecordId)
                .NotEmpty().WithMessage("سجل الحضور مطلوب");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("التاريخ مطلوب");

            RuleFor(x => x.FromTime)
                .NotEmpty().WithMessage("وقت البداية مطلوب");

            RuleFor(x => x.ToTime)
                .NotEmpty().WithMessage("وقت النهاية مطلوب")
                .GreaterThan(x => x.FromTime)
                .WithMessage("وقت النهاية يجب أن يكون بعد وقت البداية");
        }
    }
}
