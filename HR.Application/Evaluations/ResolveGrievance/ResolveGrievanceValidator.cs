using FluentValidation;

namespace HR.Application.Evaluations.ResolveGrievance
{
    internal sealed class ResolveGrievanceValidator : AbstractValidator<ResolveGrievanceCommand>
    {
        public ResolveGrievanceValidator()
        {
            RuleFor(x => x.GrievanceId)
                .NotEmpty().WithMessage("معرّف التظلم مطلوب");

            RuleFor(x => x.NewStatus)
                .NotEmpty().WithMessage("نتيجة البت مطلوبة")
                .Must(s => s == "Accepted" || s == "Rejected" || s == "PartiallyAccepted")
                .WithMessage("حالة البت غير صحيحة. القيم المسموحة: Accepted, Rejected, PartiallyAccepted");

            RuleFor(x => x.ResolutionDate)
                .NotEmpty().WithMessage("تاريخ البت مطلوب");

            RuleFor(x => x.CommitteeNotes)
                .MaximumLength(2000).WithMessage("حيثيات اللجنة لا تزيد عن 2000 حرف");
        }
    }
}
