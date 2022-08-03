namespace WorkoutGlobal.AuthorizationServiceApi.Dtos
{
    /// <summary>
    /// Represents DTO model what helps generate authentication info.
    /// </summary>
    public class DefaultRegistrationInfoDto
    {
        /// <summary>
        /// User name in system.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User phone number.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
