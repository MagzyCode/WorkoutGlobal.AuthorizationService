using System.ComponentModel;

namespace WorkoutGlobal.AuthorizationServiceApi.Models.Enums
{
    /// <summary>
    /// Represents user sport activity in life.
    /// </summary>
    public enum ActivityStatus
    {
        /// <summary>
        /// Very active user with many sports activities.
        /// </summary>
        [Description("Very active user with many sports activities.")]
        Athletic,
        /// <summary>
        /// Active user with few sports activities.
        /// </summary>
        [Description("Active user with few sports activities.")]
        Active,
        /// <summary>
        /// User have moderate activities in life such as walking, cycling and etc.
        /// </summary>
        [Description("User have moderate activities in life such as walking, cycling and etc.")]
        Moderate,
        /// <summary>
        /// User have reduces activies.
        /// </summary>
        [Description("User have reduces activies.")]
        Reduced,
        /// <summary>
        /// Very inactive person position.
        /// </summary>
        [Description("Very inactive person position.")]
        Inactive
    }
}
