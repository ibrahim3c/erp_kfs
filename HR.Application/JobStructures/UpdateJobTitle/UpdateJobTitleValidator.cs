using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.UpdateJobTitle
{
    internal sealed class UpdateJobTitleValidator : AbstractValidator<UpdateJobTitleCommand>
    {
        public UpdateJobTitleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف المسمى الوظيفي مطلوب");


            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("الكود مطلوب")
                .MaximumLength(20).WithMessage("الكود لا يتجاوز 20 حرف");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المسمى الوظيفي مطلوب")
                .MaximumLength(150).WithMessage("الاسم لا يتجاوز 150 حرف");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("الوصف لا يتجاوز 500 حرف");
        }
    }
}
