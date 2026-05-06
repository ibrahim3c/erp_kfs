using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.CreateFullEmployee
{
    internal sealed class CreateFullEmployeeValidator
     : AbstractValidator<CreateFullEmployeeCommand>
    {
        public CreateFullEmployeeValidator()
        {
            // ─── 1. البيانات الشخصية ───────────────────────────────────

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("الاسم الأول مطلوب")
                .MaximumLength(50).WithMessage("الاسم الأول لا يتجاوز 50 حرف");

            RuleFor(x => x.FatherName)
                .NotEmpty().WithMessage("اسم الأب مطلوب")
                .MaximumLength(50).WithMessage("اسم الأب لا يتجاوز 50 حرف");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("اسم العائلة مطلوب")
                .MaximumLength(50).WithMessage("اسم العائلة لا يتجاوز 50 حرف");

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("الرقم القومي مطلوب")
                .Length(14).WithMessage("الرقم القومي يجب أن يكون 14 رقماً بالضبط")
                .Matches("^[0-9]{14}$").WithMessage("الرقم القومي يجب أن يحتوي على أرقام فقط")
                .Must(BeValidNationalId).WithMessage("الرقم القومي غير صحيح (تاريخ الميلاد غير منطقي)");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("تاريخ الميلاد مطلوب")
                .LessThan(DateTime.Today.AddYears(-18))
                    .WithMessage("يجب أن يكون عمر الموظف 18 سنة على الأقل")
                .GreaterThan(new DateTime(1940, 1, 1))
                    .WithMessage("تاريخ الميلاد غير منطقي");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("النوع مطلوب")
                .Must(g => g == "ذكر" || g == "أنثى")
                    .WithMessage("النوع يجب أن يكون ذكر أو أنثى");

            RuleFor(x => x.Phone)
                .Matches(@"^01[0125][0-9]{8}$")
                    .WithMessage("رقم الموبايل غير صحيح (يجب أن يبدأ بـ 010/011/012/015)")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح");

            // ─── 2. بيانات الوظيفة ────────────────────────────────────

            RuleFor(x => x.HireDate)
                .NotEmpty().WithMessage("تاريخ التعيين مطلوب")
                .GreaterThan(new DateTime(1970, 1, 1))
                    .WithMessage("تاريخ التعيين غير منطقي")
                .LessThanOrEqualTo(DateTime.Today)
                    .WithMessage("تاريخ التعيين لا يمكن أن يكون في المستقبل");

            RuleFor(x => x.JobGradeDate)
                .GreaterThanOrEqualTo(x => x.HireDate)
                    .WithMessage("تاريخ الحصول على الدرجة لا يمكن أن يكون قبل تاريخ التعيين")
                .When(x => x.JobGradeDate.HasValue);

           

            // ─── 3. البيانات المالية (اختياري لكن مع rules منطقية) ───

            RuleFor(x => x.GrossSalary)
                .GreaterThan(0).WithMessage("الراتب الإجمالي يجب أن يكون قيمة موجبة")
                .When(x => x.GrossSalary.HasValue);

            RuleFor(x => x.Incentives)
                .GreaterThanOrEqualTo(0).WithMessage("الحوافز لا يمكن أن تكون قيمة سالبة")
                .When(x => x.Incentives.HasValue);

            RuleFor(x => x.BasicSalary2019)
                .GreaterThan(0).WithMessage("الراتب الأساسي يجب أن يكون قيمة موجبة")
                .LessThanOrEqualTo(x => x.GrossSalary ?? decimal.MaxValue)
                    .WithMessage("الراتب الأساسي لا يمكن أن يتجاوز الراتب الإجمالي")
                .When(x => x.BasicSalary2019.HasValue);

            RuleFor(x => x.InsuranceNumber)
                .MaximumLength(20).WithMessage("الرقم التأميني لا يتجاوز 20 رقم")
                .Matches("^[0-9]+$").WithMessage("الرقم التأميني يجب أن يحتوي على أرقام فقط")
                .When(x => !string.IsNullOrWhiteSpace(x.InsuranceNumber));

            RuleFor(x => x.BankAccountNumber)
                .MaximumLength(30).WithMessage("رقم الحساب البنكي لا يتجاوز 30 رقم")
                .When(x => !string.IsNullOrWhiteSpace(x.BankAccountNumber));

            // لو دخل رقم حساب لازم يدخل اسم البنك
            RuleFor(x => x.BankName)
                .NotEmpty().WithMessage("يجب إدخال اسم البنك عند إدخال رقم الحساب")
                .When(x => !string.IsNullOrWhiteSpace(x.BankAccountNumber));
        }

        // ─── Custom Rules ──────────────────────────────────────────────
        private static bool BeValidNationalId(string nationalId)
        {
            if (nationalId.Length != 14) return false;

            // استخراج تاريخ الميلاد من الرقم القومي
            int centuryDigit = nationalId[0] - '0';
            if (centuryDigit != 2 && centuryDigit != 3) return false;

            int year = (centuryDigit == 2 ? 1900 : 2000) + int.Parse(nationalId[1..3]);
            int month = int.Parse(nationalId[3..5]);
            int day = int.Parse(nationalId[5..7]);

            //  Use standard boolean operators for the 'year' check
            return month is >= 1 and <= 12
                && day is >= 1 and <= 31
                && year >= 1940 && year <= DateTime.Today.Year;
        }
    }
}
