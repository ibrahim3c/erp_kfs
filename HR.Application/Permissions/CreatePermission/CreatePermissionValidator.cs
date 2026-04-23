using FluentValidation;


namespace HR.Application.Permissions.CreatePermission
{
    internal sealed class CreatePermissionValidator
         : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("التاريخ مطلوب")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("التاريخ لا يجوز أن يكون في المستقبل");

            RuleFor(x => x.FromTime)
                .NotEmpty().WithMessage("وقت الخروج مطلوب");

            RuleFor(x => x.ToTime)
                .NotEmpty().WithMessage("وقت العودة مطلوب")
                .GreaterThan(x => x.FromTime)
                .WithMessage("وقت العودة يجب أن يكون بعد وقت الخروج");
        }
    }
}
