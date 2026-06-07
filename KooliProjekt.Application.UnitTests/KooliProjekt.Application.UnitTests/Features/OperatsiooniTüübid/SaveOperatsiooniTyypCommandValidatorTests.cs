using FluentValidation.TestHelper;
using KooliProjekt.Application.Features.OperatsiooniTüübid;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.OperatsiooniTüübid
{
    public class SaveOperatsiooniTyypCommandValidatorTests
    {
        private readonly SaveOperatsiooniTyypCommandValidator _validator = new();

        private static SaveOperatsiooniTyypCommand ValidCommand()
        {
            return new SaveOperatsiooniTyypCommand
            {
                Id = 0,
                Nimi = "Õlivahetus",
                Kirjeldus = "Mootoriõli vahetus"
            };
        }

        [Fact]
        public void Save_command_should_validate()
        {
            var result = _validator.TestValidate(ValidCommand());

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Save_should_have_error_when_id_is_negative()
        {
            var command = ValidCommand();
            command.Id = -1;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Save_should_not_have_error_when_id_is_valid()
        {
            var command = ValidCommand();
            command.Id = 1;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Save_should_have_error_when_nimi_is_empty(string nimi)
        {
            var command = ValidCommand();
            command.Nimi = nimi;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Nimi);
        }

        [Fact]
        public void Save_should_have_error_when_nimi_is_too_long()
        {
            var command = ValidCommand();
            command.Nimi = new string('A', 101);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Nimi);
        }

        [Fact]
        public void Save_should_not_have_error_when_nimi_is_valid()
        {
            var command = ValidCommand();
            command.Nimi = "Rehvide vahetus";

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Nimi);
        }

        [Fact]
        public void Save_should_have_error_when_kirjeldus_is_too_long()
        {
            var command = ValidCommand();
            command.Kirjeldus = new string('A', 256);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Kirjeldus);
        }

        [Fact]
        public void Save_should_not_have_error_when_kirjeldus_is_valid()
        {
            var command = ValidCommand();
            command.Kirjeldus = "Normaalne kirjeldus";

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Kirjeldus);
        }

        [Fact]
        public void Save_should_not_have_error_when_kirjeldus_is_null()
        {
            var command = ValidCommand();
            command.Kirjeldus = null;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Kirjeldus);
        }
    }
}