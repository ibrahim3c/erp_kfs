using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.CreateLoan
{
    public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
    {
        public CreateLoanCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty()
                .WithMessage("يجب اختيار الموظف المستفيد من السلفة.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("قيمة السلفة يجب أن تكون أكبر من صفر.");

            RuleFor(x => x.Months)
                .GreaterThan(0)
                .WithMessage("عدد أشهر التقسيط يجب أن يكون شهراً واحداً على الأقل.")
                .LessThanOrEqualTo(60) // افتراض: أقصى مدة للسداد 5 سنوات
                .WithMessage("فترة التقسيط لا يمكن أن تتجاوز 60 شهراً.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("يجب تحديد تاريخ بداية الخصم.");

            RuleFor(x => x.Reason)
                .MaximumLength(200)
                .WithMessage("سبب السلفة يجب ألا يتجاوز 200 حرف.")
                .When(x => !string.IsNullOrEmpty(x.Reason)); // التحقق من الطول فقط إذا تم إدخال سبب
        }
    }
}
