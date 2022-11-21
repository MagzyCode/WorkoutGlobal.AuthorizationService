using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.Extensions
{
    /// <summary>
    /// Represents extensions for models.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Checks if account name changed.
        /// </summary>
        /// <param name="left">Left account.</param>
        /// <param name="right">Right account.</param>
        /// <returns>Returns state if account name changed.</returns>
        public static bool IsNameChanged(this UserAccount left, UserAccount right)
        {
            return left.FirstName != right.FirstName || left.LastName != right.LastName || left.Patronymic != right.Patronymic;
        }
    }
}
