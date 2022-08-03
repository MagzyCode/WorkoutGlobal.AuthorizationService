using FluentValidation;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;

namespace WorkoutGlobal.AuthorizationServiceApi.Validators
{
    /// <summary>
    /// Validator for UserRegistrationDto.
    /// </summary>
    public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
    {
        /// <summary>
        /// Sets validation rules for registration user.
        /// </summary>
        public UserRegistrationDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(user => user.UserName)
                .NotEmpty()
                .Length(5, 40)
                .Matches(@"^([A-Za-z0-9_=+])([A-Za-z0-9_=+]){4,50}$")
                    .WithMessage("Check your '{PropertyName}' for using forbidden сharacters (@%?#<>%/) and cyrillic.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress();
            
            RuleFor(user => user.Password)
                .NotEmpty()
                .Length(6, 50)
                .Matches(@"^([A-Za-z0-9_=+])([A-Za-z0-9_=+]){5,50}$")
                    .WithMessage("Check your '{PropertyName}' for using forbidden сharacters (@%?#<>%/) and cyrillic.");

            RuleFor(user => user.PhoneNumber)
                .NotEmpty();

            RuleFor(user => user.FirstName)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(user => user.LastName)
                .NotEmpty()
                .Length(2, 100);

            When(user => user.Patronymic is not null, () =>
            {
                RuleFor(user => user.Patronymic)
                    .NotEmpty()
                    .Length(2, 100);
            });

            RuleFor(user => user.DateOfBirth)
                .NotEmpty()
                .GreaterThanOrEqualTo(new DateTime(1890, 1, 1));

            RuleFor(user => user.ResidencePlace)
                .NotEmpty()
                .Length(10, 300);

            RuleFor(user => user.Sex)
                .IsInEnum();

            When(user => user.Height is not null, () =>
            {
                RuleFor(user => user.Height)
                    .NotEmpty()
                    .InclusiveBetween(50, 270);
            });

            When(user => user.Weight is not null, () =>
            {
                RuleFor(user => user.Weight)
                    .NotEmpty()
                    .InclusiveBetween(20, 500);
            });

            RuleFor(user => user.SportsActivity)
                .IsInEnum();

            When(user => user.ClassificationNumber is not null, () =>
            {
                RuleFor(user => user.ClassificationNumber)
                    .NotEmpty()
                    .Length(8, 60);
            });
        }
    }
}
