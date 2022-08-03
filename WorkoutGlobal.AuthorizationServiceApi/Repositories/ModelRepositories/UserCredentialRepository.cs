using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.DbContext;
using WorkoutGlobal.AuthorizationServiceApi.Enums;
using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.Repositories
{
    /// <summary>
    /// Base repository for user credential model.
    /// </summary>
    public class UserCredentialRepository : BaseRepository<UserCredential>, IUserCredentialRepository
    {
        private UserManager<UserCredential> _identityUserManager;

        /// <summary>
        /// Ctor for user credential repository.
        /// </summary>
        /// <param name="userManager">Identity user manager.</param>
        /// <param name="context">Database context instance.</param>
        /// <param name="configuration">Project configuration instance.</param>
        public UserCredentialRepository(
            UserManager<UserCredential> userManager,
            AutorizationServiceContext context, 
            IConfiguration configuration) 
            : base(context, configuration)
        {
            IdentityUserManager = userManager;
        }

        /// <summary>
        /// Identity user manager.
        /// </summary>
        public UserManager<UserCredential> IdentityUserManager
        {
            get => _identityUserManager;
            private set => _identityUserManager = value ?? throw new ArgumentNullException(nameof(value), "Identity user manager cannot be null.");
        }

        /// <summary>
        /// Delete user credential.
        /// </summary>
        /// <param name="deleted">Deleted user credential.</param>
        /// <param name="deleteType">Delete type.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if second param is invalid.</exception>
        public async Task DeleteUserCredentialAsync(UserCredential deleted, DeleteType deleteType = DeleteType.Hard)
        {
            if (deleted is null)
                throw new ArgumentNullException(nameof(deleted), "Deleting user credential cannot be null.");

            switch (deleteType)
            {
                case DeleteType.Hard:
                    await IdentityUserManager.DeleteAsync(deleted);
                    break;
                case DeleteType.Soft:
                    deleted.Deleted = DateTime.Now;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deleteType));
            }
            await SaveChangesAsync();
        }

        /// <summary>
        /// Get all users credentials.
        /// </summary>
        /// <returns>Return collection of users credentials.</returns>
        public async Task<IEnumerable<UserCredential>> GetAllUserCredentialsAsync()
        {
            var users = await IdentityUserManager.Users.ToListAsync();

            return users;
        }

        /// <summary>
        /// Get user credential account.
        /// </summary>
        /// <param name="id">User credential id.</param>
        /// <returns>Returns user credential account.</returns>
        /// <exception cref="ArgumentNullException">Throws if id is null.</exception>
        public async Task<UserAccount> GetUserCredentialAccountAsync(string id)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Id cannot be null.");

            var account = await Context.UserAccounts.Where(x => x.UserCredentialsId == id).FirstOrDefaultAsync();

            return account;
        }

        /// <summary>
        /// Get user credential by id.
        /// </summary>
        /// <param name="id">User credential id.</param>
        /// <returns>Return find user credential.</returns>
        /// <exception cref="ArgumentNullException">Throws if id is null.</exception>
        public Task<UserCredential> GetUserCredentialAsync(string id)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Id cannot be null.");

            var model = IdentityUserManager.FindByIdAsync(id);

            return model;
        }

        /// <summary>
        /// Get user credential by name.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>Return find user credential.</returns>
        public async Task<UserCredential> GetUserCredentialByNameAsync(string userName)
        {
            if (userName is null)
                throw new ArgumentNullException(nameof(userName), "User name cannot be null.");

            var model = await IdentityUserManager.Users
                .Where(userCredentials => userCredentials.UserName == userName)
                .FirstOrDefaultAsync();

            return model;
        }

        /// <summary>
        /// Get all user credential roles by given id.
        /// </summary>
        /// <param name="id">User credential id.</param>
        /// <returns>Returns user roles.</returns>
        /// <exception cref="ArgumentNullException">Throws if id is null.</exception>
        public async Task<IEnumerable<string>> GetUserCredentialRolesAsync(string id)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Id cannot be null.");

            var roles = new List<string>();

            var userRoleIds = Context.UserRoles
                .Where(x => x.UserId == id)
                .Select(x => x.RoleId)
                .ToList();

            foreach (var roleId in userRoleIds)
            {
                var roleName = await Context.Roles.Where(x => x.Id == roleId).SingleAsync();

                roles.Add(roleName.Name);
            }

            return roles;
        }

        /// <summary>
        /// Raise user to trainer.
        /// </summary>
        /// <param name="id">User credential id.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if id is null.</exception>
        public async Task RaiseAsync(string id)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Id cannot be null.");

            var model = await IdentityUserManager.FindByIdAsync(id);

            await IdentityUserManager.AddToRoleAsync(model, "Trainer");

            var account = await Context.UserAccounts.Where(x => x.UserCredentialsId == model.Id).FirstOrDefaultAsync();

            account.IsStatusVerify = true;

            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates user credential.
        /// </summary>
        /// <param name="updatedCredential">Update model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if update model is null</exception>
        public async Task UpdateUserCredentialAsync(UserCredential updatedCredential)
        {
            if (updatedCredential is null)
                throw new ArgumentNullException(nameof(updatedCredential), "Updated model cannot be null.");

            await IdentityUserManager.UpdateAsync(updatedCredential);
            await SaveChangesAsync();
        }
    }
}
