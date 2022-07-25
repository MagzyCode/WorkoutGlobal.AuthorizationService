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
        /// Foreign model with account user.
        /// </summary>
        public UserAccount User { get; set; }
    }
}
