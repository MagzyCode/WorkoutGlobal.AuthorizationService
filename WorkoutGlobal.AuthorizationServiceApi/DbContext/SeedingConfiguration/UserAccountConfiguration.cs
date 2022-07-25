using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.DbContext.SeedingConfiguration
{
    /// <summary>
    /// Represents seeding of user account model.
    /// </summary>
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        /// <summary>
        /// Seeding database with initial user accounts.
        /// </summary>
        /// <param name="builder">Model builder.</param>
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.HasData(
                new UserAccount()
                {
                    Id = new Guid("07d1a783-adf7-4dcc-aa35-53abd353152d"),
                    FirstName = "Admin",
                    LastName = "Admin",
                    Patronymic = "Admin",
                    DateOfBirth = new DateTime(1970, 01, 01),
                    ResidencePlace = "Server room",
                    DateOfRegistration = DateTime.Now,
                    UserCredentialsId = "b5b84fd7-5366-44eb-9d1b-408c6a4a8926"
                });
        }
    }
}
