using System.ComponentModel;

namespace WorkoutGlobal.AuthorizationServiceApi.Enums
{
    /// <summary>
    /// Represents type of delete.
    /// </summary>
    public enum DeleteType
    {
        /// <summary>
        /// Deleting record in database.
        /// </summary>
        [Description("Deleting record in database.")]
        Hard = 1,
        /// <summary>
        /// Mark as deleted in database.
        /// </summary>
        [Description("Mark as deleted in database.")]
        Soft
    }
}
