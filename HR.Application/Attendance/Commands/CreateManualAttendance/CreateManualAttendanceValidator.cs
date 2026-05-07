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
                .NotEmpty().WithMessage("نوع الحركة مطلوب")
                .Must(x => x == MovementType.CheckIn || x == MovementType.CheckOut)
                .WithMessage("نوع الحركة يجب أن يكون in أو out");
        }
    }
}
