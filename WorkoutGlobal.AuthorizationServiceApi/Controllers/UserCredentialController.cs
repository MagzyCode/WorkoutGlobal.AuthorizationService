using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.Controllers
{
    /// <summary>
    /// User credential controller.
    /// </summary>
    [Route("api/userCredentials")]
    [ApiController]
    [Produces("application/json")]
    public class UserCredentialController : ControllerBase
    {
        private IRepositoryManager _repositoryManager;
        private IMapper _mapper;

        /// <summary>
        /// Ctor for user credential controller.
        /// </summary>
        /// <param name="repositoryManager">Repository manager.</param>
        /// <param name="mapper">Auto mapper.</param>
        public UserCredentialController(
            IRepositoryManager repositoryManager,
            IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Repository manager.
        /// </summary>
        public IRepositoryManager RepositoryManager
        {
            get => _repositoryManager;
            private set => _repositoryManager = value ?? throw new NullReferenceException(nameof(value));
        }

        /// <summary>
        /// Auto mapping helper.
        /// </summary>
        public IMapper Mapper
        {
            get => _mapper;
            private set => _mapper = value ?? throw new NullReferenceException(nameof(value));
        }

        /// <summary>
        /// Get all user credentials.
        /// </summary>
        /// <returns>Returns all user credentials.</returns>
        /// <response code="200">All user credentials successfully get.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUserCredentials()
        {
            var userCredentials = await RepositoryManager.UserCredentialRepository.GetAllUserCredentialsAsync();

            var userCredentialsDto = Mapper.Map<IEnumerable<UserCredentialDto>>(userCredentials);

            return Ok(userCredentialsDto);
        }

        /// <summary>
        /// Get user credential by id.
        /// </summary>
        /// <param name="userCredentialId">User credential id.</param>
        /// <returns>Returns find user credential.</returns>
        /// <response code="200">User credential successfully get.</response>
        /// <response code="404">User credential doesn't find by given id.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpGet("{userCredentialId}")]
        [ProducesResponseType(type: typeof(UserCredentialDto), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserCredential(string userCredentialId)
        {
            var userCredential = await RepositoryManager.UserCredentialRepository.GetUserCredentialAsync(userCredentialId);

            if (userCredential is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User don't exists.",
                    Details = new StackTrace().ToString()
                });

            var userCredentialDto = Mapper.Map<UserCredentialDto>(userCredential);

            return Ok(userCredentialDto);
        }

        /// <summary>
        /// Get user credential roles.
        /// </summary>
        /// <param name="userCredentialId">User credential id.</param>
        /// <returns>Return collection of user roles.</returns>
        /// <response code="200">User credential roles successfully get.</response>
        /// <response code="404">User credential doesn't find by given id.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpGet("{userCredentialId}/roles")]
        [ProducesResponseType(type: typeof(IEnumerable<string>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserCredentialRoles(string userCredentialId)
        {
            var userCredential = await RepositoryManager.UserCredentialRepository.GetUserCredentialAsync(userCredentialId);

            if (userCredential is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User don't exists.",
                    Details = new StackTrace().ToString()
                });

            var roles = await RepositoryManager.UserCredentialRepository.GetUserCredentialRolesAsync(userCredentialId);

            return Ok(roles);
        }

        /// <summary>
        /// Get user credential account.
        /// </summary>
        /// <param name="userCredentialId">User credential id.</param>
        /// <returns>Return user credential account.</returns>
        /// <response code="200">User credential account successfully get.</response>
        /// <response code="404">User credential doesn't find by given id.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpGet("{userCredentialId}/account")]
        [ProducesResponseType(type: typeof(UserAccountDto), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserCredentialAccount(string userCredentialId)
        {
            var userCredential = await RepositoryManager.UserCredentialRepository.GetUserCredentialAsync(userCredentialId);

            if (userCredential is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User don't exists.",
                    Details = new StackTrace().ToString()
                });

            var account = await RepositoryManager.UserCredentialRepository.GetUserCredentialAccountAsync(userCredentialId);

            var accountDto = Mapper.Map<UserAccountDto>(account);

            return Ok(accountDto);
        }


    }
}
