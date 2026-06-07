using FluentValidation;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class SaveOperatsiooniTyypCommandValidator : AbstractValidator<SaveOperatsiooniTyypCommand>
    {
        public SaveOperatsiooniTyypCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Nimi)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Kirjeldus)
                .MaximumLength(255);
        }
    }
}