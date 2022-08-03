namespace WorkoutGlobal.AuthorizationServiceApi.Contracts
{
    /// <summary>
    /// Represents base interface for managing all repositories.
    /// </summary>
    public interface IRepositoryManager
    {
        /// <summary>
        /// Authentication repository instance.
        /// </summary>
        public IAuthenticationRepository AuthenticationRepository { get; }

        /// <summary>
        /// User account repository instance.
        /// </summary>
        public IUserAccountRepository UserAccountRepository { get; }

        /// <summary>
        /// User credential repository instance.
        /// </summary>
        public IUserCredentialRepository UserCredentialRepository { get; }
    }
}
