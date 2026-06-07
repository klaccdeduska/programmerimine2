using FluentValidation;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class SaveTootajaCommandValidator : AbstractValidator<SaveTootajaCommand>
    {
        public SaveTootajaCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Nimi)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(100)
                .EmailAddress();

            RuleFor(x => x.Roll)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}