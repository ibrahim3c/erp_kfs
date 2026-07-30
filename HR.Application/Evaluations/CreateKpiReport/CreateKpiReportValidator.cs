using FluentValidation;

namespace HR.Application.Evaluations.CreateKpiReport
{
    internal sealed class CreateKpiReportValidator : AbstractValidator<CreateKpiReportCommand>
    {
        public CreateKpiReportValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.Year)
                .InclusiveBetween(2020, 2030).WithMessage("السنة غير صحيحة");

            RuleFor(x => x.EfficiencyScore)
                .InclusiveBetween(0, 30).WithMessage("درجة الكفاءة من 0 إلى 30");

            RuleFor(x => x.DisciplineScore)
                .InclusiveBetween(0, 30).WithMessage("درجة الانضباط من 0 إلى 30");

            RuleFor(x => x.AchievementScore)
                .InclusiveBetween(0, 40).WithMessage("درجة الإنجاز من 0 إلى 40");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("حالة التقرير مطلوبة")
                .Must(s => s == "Draft" || s == "Approved")
                .WithMessage("حالة التقرير غير صحيحة. القيم المسموحة: Draft, Approved");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("الملاحظات لا تزيد عن 2000 حرف");
        }
    }
}
