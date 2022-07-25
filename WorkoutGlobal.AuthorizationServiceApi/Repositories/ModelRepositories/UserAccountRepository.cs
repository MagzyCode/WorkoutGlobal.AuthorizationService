using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.DbContext;
using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.Repositories
{
    /// <summary>
    /// Base repository for user account model.
    /// </summary>
    public class UserAccountRepository : BaseRepository<UserAccount>, IUserAccountRepository
    {
        /// <summary>
        /// Ctor for account repository.
        /// </summary>
        /// <param name="context">Service database context.</param>
        /// <param name="configuration">Project configuration.</param>
        public UserAccountRepository(
            AutorizationServiceContext context, 
            IConfiguration configuration)
            : base(context, configuration)
        { }   

        /// <summary>
        /// Create account.
        /// </summary>
        /// <param name="creationAccount">Creation model.</param>
        /// <returns>Returns generated id for creation model.</returns>
        /// <exception cref="ArgumentNullException">Throws if incoming model is null.</exception>
        public async Task<Guid> CreateAccountAsync(UserAccount creationAccount)
        {
            if (creationAccount is null)
                throw new ArgumentNullException(nameof(creationAccount), "Creation model cannot be null.");

            await CreateAsync(creationAccount);
            await SaveChangesAsync();

            return creationAccount.Id;
        }

        /// <summary>
        /// Delete account.
        /// </summary>
        /// <param name="deletionAccount">Deletion model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if incoming model is null.</exception>
        public async Task DeleteAccountAsync(UserAccount deletionAccount)
        {
            if (deletionAccount is null)
                throw new ArgumentNullException(nameof(deletionAccount), "Deletion model cannot be null.");

            Delete(deletionAccount);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Get account by id.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>Returns find model.</returns>
        public async Task<UserAccount> GetAccountAsync(Guid id)
        {
            var model = await GetModelAsync(id);

            return model;
        }

        /// <summary>
        /// Get account credential.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>Returns account credential model.</returns>
        public async Task<UserCredential> GetAccountCredentialAsync(Guid id)
        {
            var account = await Context.UserAccounts.Where(x => x.Id == id).FirstOrDefaultAsync();

            var userCredential = await Context.Users.Where(x => x.Id == account.UserCredentialsId).FirstOrDefaultAsync();

            return userCredential;
        }

        /// <summary>
        /// Get all accounts.
        /// </summary>
        /// <returns>Returns collection of all accounts.</returns>
        public async Task<IEnumerable<UserAccount>> GetAllAccountsAsync()
        {
            var models = await GetAll().ToListAsync();

            return models;
        }

        /// <summary>
        /// Update account.
        /// </summary>
        /// <param name="updationAccount">Updation model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if incoming updation model is null.</exception>
        public async Task UpdateAccountAsync(UserAccount updationAccount)
        {
            if (updationAccount is null)
                throw new ArgumentNullException(nameof(updationAccount), "Updation model cannot be null.");

            Update(updationAccount);
            await SaveChangesAsync();
        }
    }
}
