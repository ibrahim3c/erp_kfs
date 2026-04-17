using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.CreateInsurancePurchase
{
    public sealed class CreateInsurancePurchaseValidator
         : AbstractValidator<CreateInsurancePurchaseCommand>
    {
        public CreateInsurancePurchaseValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty()
                .WithMessage("يجب اختيار الموظف");

            RuleFor(x => x.InsuranceAuthority)
                .NotEmpty()
                .WithMessage("يجب تحديد الجهة التأمينية")
                .MaximumLength(200)
                .WithMessage("اسم الجهة التأمينية لا يتجاوز 200 حرف");

            RuleFor(x => x.PurchasedYears)
                .GreaterThan(0)
                .WithMessage("عدد السنوات يجب أن يكون أكبر من صفر")
                .LessThanOrEqualTo(30)
                .WithMessage("عدد السنوات لا يتجاوز 30 سنة");

            RuleFor(x => x.TotalCost)
                .GreaterThan(0)
                .WithMessage("التكلفة الإجمالية يجب أن تكون أكبر من صفر");

            RuleFor(x => x.MonthlyInstallment)
                .GreaterThan(0)
                .WithMessage("القسط الشهري يجب أن يكون أكبر من صفر")
                .LessThanOrEqualTo(x => x.TotalCost)
                .WithMessage("القسط الشهري لا يجب أن يتجاوز التكلفة الإجمالية");

            RuleFor(x => x.DeductionStartDate)
                .NotEmpty()
                .WithMessage("يجب تحديد تاريخ بداية الخصم")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("تاريخ بداية الخصم لا يجوز أن يكون في الماضي");

            RuleFor(x => x.ApprovalDecisionFilePath)
                .MaximumLength(500)
                .WithMessage("مسار ملف القرار لا يتجاوز 500 حرف")
                .When(x => x.ApprovalDecisionFilePath is not null);
        }
    }
}
