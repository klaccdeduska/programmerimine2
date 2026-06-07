using FluentValidation.TestHelper;
using KooliProjekt.Application.Features.Tootajad;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Tootajad
{
    public class SaveTootajaCommandValidatorTests
    {
        private readonly SaveTootajaCommandValidator _validator = new();

        private static SaveTootajaCommand ValidCommand()
        {
            return new SaveTootajaCommand
            {
                Id = 0,
                Nimi = "Mati Maasikas",
                Email = "mati@mail.com",
                Roll = "Mehaanik"
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
            command.Nimi = "Kati Kuusk";

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Nimi);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Save_should_have_error_when_email_is_empty(string email)
        {
            var command = ValidCommand();
            command.Email = email;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Save_should_have_error_when_email_is_invalid()
        {
            var command = ValidCommand();
            command.Email = "not-email";

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Save_should_have_error_when_email_is_too_long()
        {
            var command = ValidCommand();
            command.Email = new string('a', 101) + "@mail.com";

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Save_should_not_have_error_when_email_is_valid()
        {
            var command = ValidCommand();
            command.Email = "kati@mail.com";

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Save_should_have_error_when_roll_is_empty(string roll)
        {
            var command = ValidCommand();
            command.Roll = roll;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Roll);
        }

        [Fact]
        public void Save_should_have_error_when_roll_is_too_long()
        {
            var command = ValidCommand();
            command.Roll = new string('A', 21);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Roll);
        }

        [Fact]
        public void Save_should_not_have_error_when_roll_is_valid()
        {
            var command = ValidCommand();
            command.Roll = "Admin";

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Roll);
        }
    }
}