namespace WorkoutGlobal.AuthorizationServiceApi.Dtos
{
    /// <summary>
    /// User credential DTO model for GET method.
    /// </summary>
    public class UserCredentialDto
    {
        /// <summary>
        /// Unique idetifier.
        /// </summary>
        public string Id { get; set; }

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
