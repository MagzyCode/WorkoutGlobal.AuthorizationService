using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.AuthorizationServiceApi.DbContext.SeedingConfiguration;
using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.DbContext
{
    /// <summary>
    /// Represents database context of authorization microservice project.
    /// </summary>
    public class AutorizationServiceContext : IdentityDbContext<UserCredential>
    {
        /// <summary>
        /// Ctor for set authorization microservice context options.
        /// </summary>
        /// <param name="options">Context options.</param>
        public AutorizationServiceContext(DbContextOptions options)
            : base(options)
        { }

        /// <summary>
        /// Configure the scheme needed for the identity framework.
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Seeding

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserCredentialConfiguration());
            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());

            #endregion

            #region Relations

            modelBuilder.Entity<UserCredential>()
                .HasOne(userCredentials => userCredentials.User)
                .WithOne(user => user.UserCredential)
                .HasForeignKey<UserAccount>(userCredentials => userCredentials.UserCredentialsId);

            #endregion
        }

        /// <summary>
        /// Represent table of user account.
        /// </summary>
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
