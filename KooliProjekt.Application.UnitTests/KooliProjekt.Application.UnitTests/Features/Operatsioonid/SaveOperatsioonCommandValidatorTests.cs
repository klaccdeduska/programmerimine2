using System;
using FluentValidation.TestHelper;
using KooliProjekt.Application.Features.Operatsioonid;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Operatsioonid
{
    public class SaveOperatsioonCommandValidatorTests
    {
        private readonly SaveOperatsioonCommandValidator _validator = new();

        private static SaveOperatsioonCommand ValidCommand()
        {
            return new SaveOperatsioonCommand
            {
                Id = 0,
                AutoId = 1,
                TöötajaId = 2,
                TüüpId = 3,
                Kuupäev = DateTime.Now.AddDays(-1),
                Staatus = "Valmis",
                Maksumus = 100m
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
        [InlineData(0)]
        [InlineData(-1)]
        public void Save_should_have_error_when_auto_id_is_zero_or_negative(int autoId)
        {
            var command = ValidCommand();
            command.AutoId = autoId;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.AutoId);
        }

        [Fact]
        public void Save_should_not_have_error_when_auto_id_is_valid()
        {
            var command = ValidCommand();
            command.AutoId = 1;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.AutoId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Save_should_have_error_when_tootaja_id_is_zero_or_negative(int tootajaId)
        {
            var command = ValidCommand();
            command.TöötajaId = tootajaId;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TöötajaId);
        }

        [Fact]
        public void Save_should_not_have_error_when_tootaja_id_is_valid()
        {
            var command = ValidCommand();
            command.TöötajaId = 2;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.TöötajaId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Save_should_have_error_when_tyyp_id_is_zero_or_negative(int tyypId)
        {
            var command = ValidCommand();
            command.TüüpId = tyypId;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TüüpId);
        }

        [Fact]
        public void Save_should_not_have_error_when_tyyp_id_is_valid()
        {
            var command = ValidCommand();
            command.TüüpId = 3;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.TüüpId);
        }

        [Fact]
        public void Save_should_have_error_when_kuupaev_is_in_future()
        {
            var command = ValidCommand();
            command.Kuupäev = DateTime.Now.AddDays(1);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Kuupäev);
        }

        [Fact]
        public void Save_should_not_have_error_when_kuupaev_is_not_in_future()
        {
            var command = ValidCommand();
            command.Kuupäev = DateTime.Now.AddDays(-1);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Kuupäev);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Save_should_have_error_when_staatus_is_empty(string staatus)
        {
            var command = ValidCommand();
            command.Staatus = staatus;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Staatus);
        }

        [Fact]
        public void Save_should_have_error_when_staatus_is_too_long()
        {
            var command = ValidCommand();
            command.Staatus = new string('A', 21);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Staatus);
        }

        [Fact]
        public void Save_should_have_error_when_staatus_is_not_allowed()
        {
            var command = ValidCommand();
            command.Staatus = "ValeStaatus";

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Staatus);
        }

        [Theory]
        [InlineData("Ootel")]
        [InlineData("Tegemisel")]
        [InlineData("Valmis")]
        public void Save_should_not_have_error_when_staatus_is_allowed(string staatus)
        {
            var command = ValidCommand();
            command.Staatus = staatus;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Staatus);
        }

        [Fact]
        public void Save_should_have_error_when_maksumus_is_negative()
        {
            var command = ValidCommand();
            command.Maksumus = -1m;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Maksumus);
        }

        [Fact]
        public void Save_should_not_have_error_when_maksumus_is_zero_or_positive()
        {
            var command = ValidCommand();
            command.Maksumus = 0m;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Maksumus);
        }

        [Fact]
        public void Save_should_not_have_error_when_maksumus_is_null()
        {
            var command = ValidCommand();
            command.Maksumus = null;

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Maksumus);
        }
    }
}