using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.Contracts
{
    /// <summary>
    /// Base interface for user account repository
    /// </summary>
    public interface IUserAccountRepository
    {
        /// <summary>
        /// Get all accounts.
        /// </summary>
        /// <returns>Returns collection of user accounts.</returns>
        public Task<IEnumerable<UserAccount>> GetAllAccountsAsync();

        /// <summary>
        /// Get user account by id.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>Returns user account.</returns>
        public Task<UserAccount> GetAccountAsync(Guid id);

        /// <summary>
        /// Create account.
        /// </summary>
        /// <param name="creationAccount">Creation model.</param>
        /// <returns>Returns generated id for creation model.</returns>
        public Task<Guid> CreateAccountAsync(UserAccount creationAccount);

        /// <summary>
        /// Update account.
        /// </summary>
        /// <param name="updationAccount">Updation model.</param>
        /// <returns></returns>
        public Task UpdateAccountAsync(UserAccount updationAccount);

        /// <summary>
        /// Delete account.
        /// </summary>
        /// <param name="id">Deletion id.</param>
        /// <returns></returns>
        public Task DeleteAccountAsync(Guid id);

        /// <summary>
        /// Get account credential.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>Returns find user credential.</returns>
        public Task<UserCredential> GetAccountCredentialAsync(Guid id);
    }
}
