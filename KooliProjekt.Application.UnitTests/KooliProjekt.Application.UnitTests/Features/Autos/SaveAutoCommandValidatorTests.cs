using FluentValidation.TestHelper;
using KooliProjekt.Application.Features.Autos;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Autos
{
    public class SaveAutoCommandValidatorTests
    {
        private readonly SaveAutoCommandValidator _validator = new();

        private static SaveAutoCommand ValidCommand()
        {
            return new SaveAutoCommand
            {
                Id = 0,
                Tootja = "Toyota",
                Mudel = "Corolla",
                Numbrimark = "123ABC"
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
        public void Save_should_not_have_error_when_id_is_zero_or_positive()
        {
            var command = ValidCommand();
            command.Id = 1;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Save_should_have_error_when_tootja_is_empty(string tootja)
        {
            var command = ValidCommand();
            command.Tootja = tootja;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Tootja);
        }

        [Fact]
        public void Save_should_have_error_when_tootja_is_too_long()
        {
            var command = ValidCommand();
            command.Tootja = new string('A', 101);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Tootja);
        }

        [Fact]
        public void Save_should_not_have_error_when_tootja_is_valid()
        {
            var command = ValidCommand();
            command.Tootja = "BMW";

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Tootja);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Save_should_have_error_when_mudel_is_empty(string mudel)
        {
            var command = ValidCommand();
            command.Mudel = mudel;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Mudel);
        }

        [Fact]
        public void Save_should_have_error_when_mudel_is_too_long()
        {
            var command = ValidCommand();
            command.Mudel = new string('A', 101);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Mudel);
        }

        [Fact]
        public void Save_should_not_have_error_when_mudel_is_valid()
        {
            var command = ValidCommand();
            command.Mudel = "X5";

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Mudel);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Save_should_have_error_when_numbrimark_is_empty(string numbrimark)
        {
            var command = ValidCommand();
            command.Numbrimark = numbrimark;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Numbrimark);
        }

        [Fact]
        public void Save_should_have_error_when_numbrimark_is_too_long()
        {
            var command = ValidCommand();
            command.Numbrimark = new string('A', 16);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Numbrimark);
        }

        [Fact]
        public void Save_should_not_have_error_when_numbrimark_is_valid()
        {
            var command = ValidCommand();
            command.Numbrimark = "456DEF";

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Numbrimark);
        }
    }
}