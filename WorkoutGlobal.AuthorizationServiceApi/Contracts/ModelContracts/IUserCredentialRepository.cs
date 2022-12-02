using WorkoutGlobal.AuthorizationServiceApi.Enums;
using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.Contracts
{
    /// <summary>
    /// Base interface for user credential repository.
    /// </summary>
    public interface IUserCredentialRepository
    {
        /// <summary>
        /// Get all user credentials.
        /// </summary>
        /// <param name="trackChanges">Track change state.</param>
        /// <returns>Returns collection of all user credentials.</returns>
        public Task<IEnumerable<UserCredential>> GetAllUserCredentialsAsync(bool trackChanges = true);

        /// <summary>
        /// Get single user credential by id.
        /// </summary>
        /// <param name="id">User credential id.</param>
        /// <param name="trackChanges">Track change state.</param>
        /// <returns>Returns find user credential by given id.</returns>
        public Task<UserCredential> GetUserCredentialAsync(string id, bool trackChanges = true);

        /// <summary>
        /// Get single user credential by user name.
        /// </summary>
        /// <param name="userName">User name in system.</param>
        /// <param name="trackChanges">Track change state.</param>
        /// <returns>Returns find user credential by given user name.</returns>
        public Task<UserCredential> GetUserCredentialByNameAsync(string userName, bool trackChanges = true);

        /// <summary>
        /// Updates user credential by given model.
        /// </summary>
        /// <param name="updatedCredential">Updation model.</param>
        /// <returns></returns>
        public Task UpdateUserCredentialAsync(UserCredential updatedCredential);

        /// <summary>
        /// Delete user credential with given id.
        /// </summary>
        /// <param name="deleted">Deleted user credential.</param>
        /// <param name="deleteType">Set delete action.</param>
        /// <returns></returns>
        public Task DeleteUserCredentialAsync(UserCredential deleted, DeleteType deleteType = DeleteType.Hard);

        /// <summary>
        /// Get all user credential roles.
        /// </summary>
        /// <param name="id">User credential id.</param>
        /// <returns>Return collection of user credential roles.</returns>
        public Task<IEnumerable<string>> GetUserCredentialRolesAsync(string id);

        /// <summary>
        /// Raise user credential to trainer.
        /// </summary>
        /// <param name="id">User credential id.</param>
        /// <returns></returns>
        public Task RaiseAsync(string id);

        /// <summary>
        /// Get user credential account.
        /// </summary>
        /// <param name="id">User credential id.</param>
        /// <param name="trackChanges">Track change state.</param>
        /// <returns>Returns find account.</returns>
        public Task<UserAccount> GetUserCredentialAccountAsync(string id, bool trackChanges = true);
    }
}
