using Microsoft.AspNetCore.Identity;

namespace WorkoutGlobal.AuthorizationServiceApi.Models
{
    /// <summary>
    /// Represents user credentials model.
    /// </summary>
    public class UserCredential : IdentityUser
    {
        /// <summary>
        /// Salt for user password.
        /// </summary>
        public string PasswordSalt { get; set; }

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

        /// <summary>
        /// Foreign model with account user.
        /// </summary>
        public UserAccount User { get; set; }
    }
}
