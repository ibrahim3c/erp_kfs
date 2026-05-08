using FluentValidation;

namespace HR.Application.Attendance.Commands.CreateManualAttendance
{
    internal sealed class CreateManualAttendanceValidator
        : AbstractValidator<CreateManualAttendanceCommand>
    {
        public CreateManualAttendanceValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("التاريخ مطلوب");

            RuleFor(x => x.MovementType)
            .IsInEnum().WithMessage("نوع الحركة غير صحيح أو غير مدعوم");
        }
    }
}
