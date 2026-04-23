using FluentValidation;


namespace HR.Application.Permissions.CreateLateEntry
{
    internal sealed class CreateLateEntryValidator
       : AbstractValidator<CreateLateEntryCommand>
    {
        public CreateLateEntryValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("تاريخ التأخير مطلوب")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("التاريخ لا يجوز أن يكون في المستقبل");

            RuleFor(x => x.ActualArrivalTime)
                .NotEmpty().WithMessage("وقت الحضور مطلوب")
                .GreaterThan(new TimeSpan(8, 0, 0))
                .WithMessage("وقت الحضور يجب أن يكون بعد 8:00 صباحاً");
        }
    }
}
