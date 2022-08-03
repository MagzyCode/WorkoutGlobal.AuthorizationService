using WorkoutGlobal.AuthorizationServiceApi.Enums;

namespace WorkoutGlobal.AuthorizationServiceApi.Dtos
{
    /// <summary>
    /// Represents DTO model for user registration.
    /// </summary>
    public class UserRegistrationDto
    {
        /// <summary>
        /// User name for registration.
        /// </summary>
        /// <example>
        /// Anonymous
        /// </example>
        public string UserName { get; set; }

        /// <summary>
        /// User email for registration.
        /// </summary>
        /// <example>
        /// aaaaaa@mail.com
        /// </example>
        public string Email { get; set; }

        /// <summary>
        /// User password for registration.
        /// </summary>
        /// <example>
        /// password_1
        /// </example>
        public string Password { get; set; }

        /// <summary>
        /// User phone number.
        /// </summary>
        /// <example>
        /// +375250000001
        /// </example>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User first name.
        /// </summary>
        /// <example>
        /// Mikhail
        /// </example>
        public string FirstName { get; set; }

        /// <summary>
        /// User last name.
        /// </summary>
        /// <example>
        /// Kazarevich
        /// </example>
        public string LastName { get; set; }

        /// <summary>
        /// User patronymic.
        /// </summary>
        /// <example>
        /// Andreevich
        /// </example>
        public string Patronymic { get; set; }

        /// <summary>
        /// User date of birth.
        /// </summary>
        /// <example>
        /// 2002-05-30T09:00:00
        /// </example>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// User residence place.
        /// </summary>
        /// <example>
        /// Republic of Belarus, Gomel, Pedchenko street, 12, 120
        /// </example>
        public string ResidencePlace { get; set; }

        /// <summary>
        /// User sex.
        /// </summary>
        /// <example>
        /// 0
        /// </example>
        public Sex Sex { get; set; }

        /// <summary>
        /// User height.
        /// </summary>
        /// <example>
        /// 186
        /// </example>
        public double? Height { get; set; }

        /// <summary>
        /// User weight.
        /// </summary>
        /// <example>
        /// 100
        /// </example>
        public double? Weight { get; set; }

        /// <summary>
        /// User's attitude to sports.
        /// </summary>
        /// <example>
        /// 0
        /// </example>
        public ActivityStatus SportsActivity { get; set; }

        /// <summary>
        /// Trainer official classification number.
        /// </summary>
        /// <example>
        /// HR123456789GT258
        /// </example>
        public string ClassificationNumber { get; set; }
    }
}
