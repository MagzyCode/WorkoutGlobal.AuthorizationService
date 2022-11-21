using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.DbContext;

namespace WorkoutGlobal.AuthorizationServiceApi.Repositories
{
    /// <summary>
    /// Represents base repository for all model repositories.
    /// </summary>
    /// <typeparam name="TModel">Model type.</typeparam>
    /// <typeparam name="TId">Id type.</typeparam>
    public abstract class BaseRepository<TModel, TId> : IBaseRepository<TModel, TId>
        where TModel : class, IModel<TId>
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
            private set => _context = value;
        }

        /// <summary>
        /// Service configuration.
        /// </summary>
        public IConfiguration Configuration
        {
            get => _configuration;
            private set => _configuration = value;
        }

        /// <summary>
        /// Asynchronous creation of model.
        /// </summary>
        /// <param name="model">Creation model.</param>
        /// <returns>A task that represents asynchronous Create operation.</returns>
        public async Task<TId> CreateAsync(TModel model)
        {
            await Context.Set<TModel>().AddAsync(model);

            return model.Id;        
        }

        /// <summary>
        /// Deleting model.
        /// </summary>
        /// <param name="id">Deleting model id.</param>
        public async Task DeleteAsync(TId id)
        {
            var model = await GetModelAsync(id);

            Context.Set<TModel>().Remove(model);
        }

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <param name="trackChanges">Track changes state.</param>
        /// <returns>Collection of all models.</returns>
        public IQueryable<TModel> GetAll(bool trackChanges = true)
        {
            var result = trackChanges
                ? Context.Set<TModel>()
                : Context.Set<TModel>().AsNoTracking();

            return result;
        }

        /// <summary>
        /// Asynchronous getting of model.
        /// </summary>
        /// <param name="id">Id of getting model.</param>
        /// <param name="trackChanges">Track changes state.</param>
        /// <returns>Single model.</returns>
        public async Task<TModel> GetModelAsync(TId id, bool trackChanges = true)
        {
            var model = trackChanges
                ? await Context.Set<TModel>().FindAsync(id)
                : await Context.Set<TModel>().AsNoTracking()
                    .Where(x => x.Id.Equals(id))
                    .FirstOrDefaultAsync();

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
