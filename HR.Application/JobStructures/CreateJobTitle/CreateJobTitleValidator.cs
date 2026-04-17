using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.CreateJobTitle
{
    internal sealed class CreateJobTitleValidator : AbstractValidator<CreateJobTitleCommand>
    {
        public CreateJobTitleValidator()
        {
            RuleFor(x => x.FunctionalGroupId)
                .NotEmpty().WithMessage("يجب اختيار المجموعة الوظيفية");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("الكود مطلوب")
                .MaximumLength(20).WithMessage("الكود لا يتجاوز 20 حرف");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المسمى الوظيفي مطلوب")
                .MaximumLength(150).WithMessage("الاسم لا يتجاوز 150 حرف");
        }
    }
}
