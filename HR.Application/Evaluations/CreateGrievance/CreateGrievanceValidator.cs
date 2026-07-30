using FluentValidation;

namespace HR.Application.Evaluations.CreateGrievance
{
    internal sealed class CreateGrievanceValidator : AbstractValidator<CreateGrievanceCommand>
    {
        public CreateGrievanceValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.GrievanceType)
                .NotEmpty().WithMessage("نوع التظلم مطلوب");

            RuleFor(x => x.ComplainedDecisionNumber)
                .NotEmpty().WithMessage("رقم القرار مطلوب")
                .MaximumLength(100).WithMessage("رقم القرار لا يزيد عن 100 حرف");

            RuleFor(x => x.ComplainedDecisionDate)
                .NotEmpty().WithMessage("تاريخ القرار مطلوب");

            RuleFor(x => x.SubmissionDate)
                .NotEmpty().WithMessage("تاريخ التقديم مطلوب");

            RuleFor(x => x.Reasons)
                .NotEmpty().WithMessage("أسباب التظلم مطلوبة")
                .MaximumLength(2000).WithMessage("الأسباب لا تزيد عن 2000 حرف");
        }
    }
}
