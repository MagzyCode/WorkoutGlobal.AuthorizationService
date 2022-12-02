using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.DbContext;
using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.Repositories
{
    /// <summary>
    /// Base repository for user account model.
    /// </summary>
    public class UserAccountRepository : BaseRepository<UserAccount, Guid>, IUserAccountRepository
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
        /// <param name="id">Deletion id.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if incoming model is null.</exception>
        public async Task DeleteAccountAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Deletion model cannot be null.", nameof(id));

            await DeleteAsync(id);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Get user account by id.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <param name="trackChanges">Track changes state.</param>
        /// <returns>Returns user account.</returns>
        /// <exception cref="ArgumentException">Throws if account id is null.</exception>
        public async Task<UserAccount> GetAccountAsync(Guid id, bool trackChanges = true)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Deletion model cannot be null.", nameof(id));

            var model = await GetModelAsync(id, trackChanges);

            return model;
        }

        /// <summary>
        /// Get account credential.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <param name="trackChanges">Track changes state.</param>
        /// <returns>Returns account credential model.</returns>
        public async Task<UserCredential> GetAccountCredentialAsync(Guid id, bool trackChanges = true)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Deletion model cannot be null.", nameof(id));

            var account = await Context.UserAccounts.Where(x => x.Id == id).FirstOrDefaultAsync();

            var userCredential = trackChanges
                ? await Context.Users.Where(x => x.Id == account.UserCredentialsId).FirstOrDefaultAsync()
                : await Context.Users.AsNoTracking().Where(x => x.Id == account.UserCredentialsId).FirstOrDefaultAsync();

            return userCredential;
        }

        /// <summary>
        /// Get all accounts.
        /// </summary>
        /// <param name="trackChanges">Track changes state.</param>
        /// <returns>Returns collection of all accounts.</returns>
        public async Task<IEnumerable<UserAccount>> GetAllAccountsAsync(bool trackChanges = true)
        {
            var models = await GetAll(trackChanges).ToListAsync();

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
