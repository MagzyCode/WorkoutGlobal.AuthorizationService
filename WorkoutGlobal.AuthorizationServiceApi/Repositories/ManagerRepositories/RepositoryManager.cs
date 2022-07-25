using WorkoutGlobal.AuthorizationServiceApi.Contracts;

namespace WorkoutGlobal.AuthorizationServiceApi.Repositories
{
    /// <summary>
    /// Represents base repository manager class for manage all model repositories.
    /// </summary>
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserCredentialRepository _userCredentialRepository;

        /// <summary>
        /// Ctor for repository manager.
        /// </summary>
        /// <param name="authenticationRepository">Authentication repository.</param>
        /// <param name="userAccountRepository">User account repository.</param>
        /// <param name="userCredentialRepository">User credential repository.</param>
        public RepositoryManager(
            IAuthenticationRepository authenticationRepository,
            IUserAccountRepository userAccountRepository,
            IUserCredentialRepository userCredentialRepository)
        {
            _authenticationRepository = authenticationRepository;
            _userAccountRepository = userAccountRepository;
            _userCredentialRepository = userCredentialRepository;
        }

        /// <summary>
        /// Authentication repository instance.
        /// </summary>
        public IAuthenticationRepository AuthenticationRepository => _authenticationRepository;

        /// <summary>
        /// User account repository instance.
        /// </summary>
        public IUserAccountRepository UserAccountRepository => _userAccountRepository;

        /// <summary>
        /// User credential repository instance.
        /// </summary>
        public IUserCredentialRepository UserCredentialRepository => _userCredentialRepository;
    }
}
