using WorkoutGlobal.AuthorizationServiceApi.DbContext;

namespace WorkoutGlobal.AuthorizationServiceApi.Contracts
{
    /// <summary>
    /// Base interface for all repositories.
    /// </summary>
    /// <typeparam name="TModel">Model class.</typeparam>
    /// <typeparam name="TId">Key type.</typeparam>
    public interface IBaseRepository<TModel, TId>
        where TModel : class, IModel<TId>
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
        public Task<TId> CreateAsync(TModel model);

        /// <summary>
        /// General update action for existed model. Id of model is required.
        /// </summary>
        /// <param name="model">Updated model.</param>
        public void Update(TModel model);

        /// <summary>
        /// Gerenal deletion of existed model.
        /// </summary>
        /// <param name="id">Deleting model id.</param>
        public Task DeleteAsync(TId id);

        /// <summary>
        /// General getting of all models.
        /// </summary>
        /// <param name="trackChanges">Track changes state.</param>
        /// <returns>IQueryable list of models.</returns>
        public IQueryable<TModel> GetAll(bool trackChanges);

        /// <summary>
        /// Gerenal getting of single model by id.
        /// </summary>
        /// <param name="id">Model id.</param>
        /// <param name="trackChanges">Track changes state.</param>
        /// <returns>Existed model.</returns>
        public Task<TModel> GetModelAsync(TId id, bool trackChanges);

        /// <summary>
        /// Save all changes in database.
        /// </summary>
        /// <returns>A task that represent asynchronous Save operation in database.</returns>
        public Task SaveChangesAsync();
    }
}
