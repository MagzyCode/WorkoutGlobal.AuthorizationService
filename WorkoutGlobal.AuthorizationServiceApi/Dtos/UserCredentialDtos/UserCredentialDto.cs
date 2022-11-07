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
        /// Salt for user password.
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Hashed representation of the password for this user.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Mark for deleted users.
        /// </summary>
        public DateTime? Deleted { get; set; }

        /// <summary>
        /// Refresh token for create access token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Expiration time of refresh token.
        /// </summary>
        public DateTime RefreshTokenExpiredDate { get; set; }
    }
}
