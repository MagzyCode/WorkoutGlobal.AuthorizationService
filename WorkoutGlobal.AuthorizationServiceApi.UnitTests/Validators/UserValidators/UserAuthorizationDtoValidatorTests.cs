using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System.Threading.Tasks;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Validators;
using Xunit;

namespace WorkoutGlobal.AuthorizationServiceApi.UnitTests.Validators
{
    public class UserAuthorizationDtoValidatorTests
    {
        private readonly UserAuthorizationDtoValidator _validator = new();
        private readonly Fixture _fixture = new();
        
        [Fact]
        public async Task ModelState_NullUserCredentials_ReturnValidationResult()
        {
            // arrange
            var userAuthorizationDto = new UserAuthorizationDto();

            // act
            var validationResult = await _validator.ValidateAsync(userAuthorizationDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(2);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_EmptyUserCredentials_ReturnValidationResult()
        {
            // arrange
            var userAuthorizationDto = _fixture.Build<UserAuthorizationDto>()
                .With(user => user.UserName, string.Empty)
                .With(user => user.Password, string.Empty)
                .Create();

            // act
            var result = await _validator.ValidateAsync(userAuthorizationDto);

            // assert
            result.Should().BeOfType(typeof(ValidationResult));
            result.Should().NotBeNull();
            result.Errors.Should().HaveCount(2);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_ValidUserCredentials_ReturnValidationResult()
        {
            // arrange
            var userAuthorizationDto = _fixture.Create<UserAuthorizationDto>();

            // act
            var result = await _validator.ValidateAsync(userAuthorizationDto);

            // assert
            result.Should().BeOfType(typeof(ValidationResult));
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            result.IsValid.Should().BeTrue();
        }
    }
}
