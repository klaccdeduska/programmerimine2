using System;
using FluentValidation;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class SaveOperatsioonCommandValidator : AbstractValidator<SaveOperatsioonCommand>
    {
        private static readonly string[] AllowedStatuses =
        {
            "Ootel",
            "Tegemisel",
            "Valmis"
        };

        public SaveOperatsioonCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.AutoId)
                .GreaterThan(0);

            RuleFor(x => x.TöötajaId)
                .GreaterThan(0);

            RuleFor(x => x.TüüpId)
                .GreaterThan(0);

            RuleFor(x => x.Kuupäev)
                .Must(date => date <= DateTime.Now)
                .WithMessage("Kuupäev ei tohi olla tulevikus.");

            RuleFor(x => x.Staatus)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Staatus)
                .Must(status => Array.Exists(AllowedStatuses, s => s == status))
                .When(x => !string.IsNullOrWhiteSpace(x.Staatus))
                .WithMessage("Staatus peab olema Ootel, Tegemisel või Valmis.");

            RuleFor(x => x.Maksumus)
                .Must(value => !value.HasValue || value.Value >= 0)
                .WithMessage("Maksumus peab olema suurem või võrdne nulliga.");
        }
    }
}