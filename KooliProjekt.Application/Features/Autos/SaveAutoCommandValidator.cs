using FluentValidation;

namespace KooliProjekt.Application.Features.Autos
{
    public class SaveAutoCommandValidator : AbstractValidator<SaveAutoCommand>
    {
        public SaveAutoCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Tootja)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Mudel)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Numbrimark)
                .NotEmpty()
                .MaximumLength(15);
        }
    }
}