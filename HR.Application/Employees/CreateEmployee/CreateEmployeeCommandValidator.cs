using FluentValidation;
namespace HR.Application.Employees.CreateEmployee
{
    public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("اسم الموظف مطلوب ولا يمكن أن يكون فارغاً")
                .MaximumLength(150);

            RuleFor(e => e.NationalId)
                .NotEmpty().WithMessage("الرقم القومي مطلوب")
                .Length(14).WithMessage("الرقم القومي غير صحيح، يجب أن يتكون من 14 رقماً");

            RuleFor(e => e.HireDate)
                .NotEmpty().WithMessage("تاريخ التعيين مطلوب");

            RuleFor(e => e.Email)
                .EmailAddress().WithMessage("البريد الإلكتروني غير صالح")
                .When(e => !string.IsNullOrWhiteSpace(e.Email));
        }
    }
}
