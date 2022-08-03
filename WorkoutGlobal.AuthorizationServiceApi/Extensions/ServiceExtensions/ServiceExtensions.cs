using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.DbContext;
using WorkoutGlobal.AuthorizationServiceApi.Filters;
using WorkoutGlobal.AuthorizationServiceApi.Models;
using WorkoutGlobal.AuthorizationServiceApi.Repositories;

namespace WorkoutGlobal.AuthorizationServiceApi.Extensions
{
    /// <summary>
    /// Base class for all service extensions.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configure database settings.
        /// </summary>
        /// <param name="services">Project services.</param>
        /// <param name="configuration">Project configuration.</param>
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<AutorizationServiceContext>(
                opts => opts.UseSqlServer(configuration.GetConnectionString("AuthorizationServiceConnectionString"),
                b => b.MigrationsAssembly("WorkoutGlobal.AuthorizationServiceApi")));

        /// <summary>
        /// Configure identity.
        /// </summary>
        /// <param name="services">Project services.</param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<UserCredential>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole),
                builder.Services);
            builder.AddEntityFrameworkStores<AutorizationServiceContext>().AddDefaultTokenProviders();
        }

        /// <summary>
        /// Configure instances of attributes.
        /// </summary>
        /// <param name="services">Project services.</param>
        public static void ConfigureAttributes(this IServiceCollection services) => services.AddScoped<ModelValidationFilterAttribute>();

        /// <summary>
        /// Configure instances of repository classes.
        /// </summary>
        /// <param name="services">Project services.</param>
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();

            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    }
}
