namespace WorkoutGlobal.AuthorizationServiceApi.Contracts
{
    /// <summary>
    /// Model base interface.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IModel<TId>
    {
        /// <summary>
        /// Model id.
        /// </summary>
        public TId Id { get; set; }
    }
}
