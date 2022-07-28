using FluentValidation;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;

namespace WorkoutGlobal.AuthorizationServiceApi.Validators
{
    /// <summary>
    /// Validator for UserAuthorizationDto class.
    /// </summary>
    public class UserAuthorizationDtoValidator : AbstractValidator<UserAuthorizationDto>
    {
        /// <summary>
        /// Sets rules for authorization model.
        /// </summary>
        public UserAuthorizationDtoValidator()
        {
            RuleFor(user => user.UserName)
                .NotEmpty();

            RuleFor(user => user.Password)
                .NotEmpty();
        }
    }
}
