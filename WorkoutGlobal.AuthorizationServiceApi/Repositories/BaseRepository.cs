using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.DbContext;

namespace WorkoutGlobal.AuthorizationServiceApi.Repositories
{
    /// <summary>
    /// Represents base repository for all model repositories.
    /// </summary>
    /// <typeparam name="TModel">Model type.</typeparam>
    public abstract class BaseRepository<TModel> : IBaseRepository<TModel>
        where TModel : class
    {
        private AutorizationServiceContext _context;
        private IConfiguration _configuration;

        /// <summary>
        /// Ctor for base repository/
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="configuration">Project configuration.</param>
        public BaseRepository(AutorizationServiceContext context, IConfiguration configuration)
        {
            Configuration = configuration;
            Context = context;
        }

        /// <summary>
        /// Service database context.
        /// </summary>
        public AutorizationServiceContext Context
        {
            get => _context;
            private set => _context = value ?? throw new NullReferenceException("Database context cannot be null.");
        }

        /// <summary>
        /// Service configuration.
        /// </summary>
        public IConfiguration Configuration
        {
            get => _configuration;
            private set => _configuration = value ?? throw new NullReferenceException("Configuration cannot be null.");
        }

        /// <summary>
        /// Asynchronous creation of model.
        /// </summary>
        /// <param name="model">Creation model.</param>
        /// <returns>A task that represents asynchronous Create operation.</returns>
        public async Task CreateAsync(TModel model)
        {
            await Context.Set<TModel>().AddAsync(model);
        }

        /// <summary>
        /// Deleting model.
        /// </summary>
        /// <param name="model">Deleting model.</param>
        public void Delete(TModel model)
        {
            Context.Set<TModel>().Remove(model);
        }

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <returns>Collection of all models.</returns>
        public IQueryable<TModel> GetAll()
        {
            var result = Context.Set<TModel>();

            return result;
        }

        /// <summary>
        /// Asynchronous getting of model.
        /// </summary>
        /// <param name="id">Id of getting model.</param>
        /// <returns>Single model.</returns>
        public async Task<TModel> GetModelAsync(Guid id)
        {
            var model = await Context.Set<TModel>().FindAsync(id);

            return model;
        }

        /// <summary>
        /// Update model.
        /// </summary>
        /// <param name="model">Updated model.</param>
        public void Update(TModel model)
        {
            Context.Set<TModel>().Update(model);
        }

        /// <summary>
        /// Asynchronous save changes in database.
        /// </summary>
        /// <returns>A task that represents asynchronous SaveChanges operaion.</returns>
        public async Task SaveChangesAsync() => await Context.SaveChangesAsync();
    }
}
