using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Validators;
using Xunit;

namespace WorkoutGlobal.AuthorizationServiceApi.UnitTests.Validators
{
    public class UserRegistrationDtoValidatorTests
    {
        private readonly UserRegistrationDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task ModelState_NullRegistrationCredentials_ReturnValidationResult()
        {
            // arrange
            var userRegistrationUserDto = new UserRegistrationDto();

            // act
            var validationResult = await _validator.ValidateAsync(userRegistrationUserDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(8);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_IncorrectUserNameAndPasswordLength_ReturnValidationResult()
        {
            // arrange
            var userRegistrationUserDto = _fixture.Build<UserRegistrationDto>()
                .With(user => user.UserName, "Name")
                .With(user => user.Password, "123")
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(userRegistrationUserDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Count.Should().BeGreaterThanOrEqualTo(2);
            var errorsMessage = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            errorsMessage.Should().Contain(
                expected: new[] { 
                    "'User Name' must be between 5 and 40 characters. You entered 4 characters.", 
                    "'Password' must be between 6 and 50 characters. You entered 3 characters." });
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_IncorrectUserNameAndPasswordPattern_ReturnValidationResult()
        {
            // arrange
            var userRegistrationUserDto = _fixture.Build<UserRegistrationDto>()
                .With(user => user.UserName, "alpha bet")
                .With(user => user.Password, "zqwert 123")
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(userRegistrationUserDto);
 
            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Count.Should().BeGreaterThanOrEqualTo(2);
            var errorsMessage = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            errorsMessage.Should().Contain(
                expected: new[] {
                    "Check your 'User Name' for using forbidden сharacters (@%?#<>%/) and cyrillic.",
                    "Check your 'Password' for using forbidden сharacters (@%?#<>%/) and cyrillic." });
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_IncorrectUserEmail_ReturnValidationResult()
        {
            // arrange
            var userRegistrationUserDto = _fixture.Build<UserRegistrationDto>()
                .With(user => user.Email, "@mail.com")
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(userRegistrationUserDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Count.Should().BeGreaterThanOrEqualTo(1);
            var errorsMessage = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            errorsMessage.Should().Contain(
                expected: new[] {
                    "'Email' is not a valid email address."});
            validationResult.IsValid.Should().BeFalse();
        }
    }
}
