using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.CreateJobGrade
{
    internal sealed class CreateJobGradeValidator : AbstractValidator<CreateJobGradeCommand>
    {
        public CreateJobGradeValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("الكود مطلوب")
                .MaximumLength(20).WithMessage("الكود لا يتجاوز 20 حرف");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("الاسم مطلوب")
                .MaximumLength(100).WithMessage("الاسم لا يتجاوز 100 حرف");

            RuleFor(x => x.GradeLevel)
                .GreaterThan(0).WithMessage("مستوى الدرجة يجب أن يكون أكبر من صفر");

            RuleFor(x => x.YearsNo)
                .GreaterThanOrEqualTo(0).WithMessage("عدد السنوات لا يجوز أن يكون سالب");
        }
    }
}
