using WorkoutGlobal.AuthorizationServiceApi.DbContext;

namespace WorkoutGlobal.AuthorizationServiceApi.Contracts
{
    /// <summary>
    /// Base interface for all repositories.
    /// </summary>
    /// <typeparam name="TModel">Model class.</typeparam>
    public interface IBaseRepository<TModel>
    {
        /// <summary>
        /// Project common database context.
        /// </summary>
        public AutorizationServiceContext Context { get; }

        /// <summary>
        /// Project configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gerenal creation of new model.
        /// </summary>
        /// <param name="model">Creation model.</param>
        /// <returns>A task that represents asynchronous Create operation.</returns>
        public Task CreateAsync(TModel model);

        /// <summary>
        /// General update action for existed model.
        /// </summary>
        /// <param name="model">Updated model.</param>
        public void Update(TModel model);

        /// <summary>
        /// Gerenal deletion of existed model.
        /// </summary>
        /// <param name="model">Deleting model.</param>
        public void Delete(TModel model);

        /// <summary>
        /// General getting of all models.
        /// </summary>
        /// <returns>IQueryable list of models.</returns>
        public IQueryable<TModel> GetAll();

        /// <summary>
        /// Gerenal getting of single model by id.
        /// </summary>
        /// <param name="id">Model id.</param>
        /// <returns>Existed model.</returns>
        public Task<TModel> GetModelAsync(Guid id);

        /// <summary>
        /// Save all changes in database.
        /// </summary>
        /// <returns>A task that represent asynchronous Save operation in database.</returns>
        public Task SaveChangesAsync();
    }
}
