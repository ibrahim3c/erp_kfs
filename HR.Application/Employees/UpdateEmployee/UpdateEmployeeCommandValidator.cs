using FluentValidation;
namespace HR.Application.Employees.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("معرف الموظف مطلوب");
            RuleFor(x => x.Name).NotEmpty().WithMessage("الاسم الأول مطلوب");
            RuleFor(x => x.Code).NotEmpty().WithMessage("الاسم الأخير مطلوب");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("البريد الإلكتروني غير صالح");
            RuleFor(x => x.HireDate).NotEmpty().WithMessage("تاريخ التوظيف مطلوب");
        }
    }
}
