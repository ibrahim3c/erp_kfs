using FluentValidation;

namespace HR.Application.Absence.SettleAbsence
{
    internal sealed class SettleAbsenceValidator : AbstractValidator<SettleAbsenceCommand>
    {
        public SettleAbsenceValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.ActionType)
                .NotEmpty().WithMessage("نوع التسوية مطلوب")
                .Must(t => t == "DeductBalance" || t == "DeductCasual" || t == "SalaryPenalty")
                .WithMessage("نوع التسوية غير صحيح. القيم المسموحة: DeductBalance, DeductCasual, SalaryPenalty");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).WithMessage("رقم الشهر غير صحيح");

            RuleFor(x => x.Year)
                .InclusiveBetween(2020, 2030).WithMessage("السنة غير صحيحة");
        }
    }
}
