using FluentValidation;

namespace HR.Application.Legal.CreateRuling
{
    internal sealed class CreateRulingValidator : AbstractValidator<CreateRulingCommand>
    {
        public CreateRulingValidator()
        {
            RuleFor(x => x.CaseNumber)
                .NotEmpty().WithMessage("رقم الدعوى مطلوب")
                .MaximumLength(100).WithMessage("رقم الدعوى لا يزيد عن 100 حرف");

            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("السنة القضائية مطلوبة")
                .MaximumLength(50).WithMessage("السنة لا تزيد عن 50 حرف");

            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.EmployeeName)
                .NotEmpty().WithMessage("اسم الموظف مطلوب")
                .MaximumLength(200).WithMessage("الاسم لا يزيد عن 200 حرف");

            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("منطوق الحكم مطلوب")
                .MaximumLength(2000).WithMessage("الملخص لا يزيد عن 2000 حرف");

            RuleFor(x => x.ExecutionType)
                .IsInEnum().WithMessage("نوع التنفيذ غير صحيح");
        }
    }
}
