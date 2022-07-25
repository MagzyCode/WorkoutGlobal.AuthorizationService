using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.Filters;
using WorkoutGlobal.AuthorizationServiceApi.Models;
using WorkoutGlobal.AuthorizationServiceApi.Models.Dto;

namespace WorkoutGlobal.AuthorizationServiceApi.Controllers
{
    /// <summary>
    /// Represents controller for authorization and registration.
    /// </summary>
    [Route("api/authentication")]
    [ApiController]
    [Produces("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private IRepositoryManager _repositoryManager;

        /// <summary>
        /// Ctor for authentication controller.
        /// </summary>
        /// <param name="repositoryManager">Repository manager instance.</param>
        public AuthenticationController(IRepositoryManager repositoryManager)
        {
            RepositoryManager = repositoryManager;
        }

        /// <summary>
        /// Authentication repository property.
        /// </summary>
        public IRepositoryManager RepositoryManager
        {
            get => _repositoryManager;
            private set => _repositoryManager = value ?? throw new NullReferenceException(nameof(value));
        }

        /// <summary>
        /// Authenticate user credentials.
        /// </summary>
        /// <param name="userAuthorizationDto">User credentials.</param>
        /// <returns>If credentials are correct sent JWT-token, otherwise sent Unauthorized.</returns>
        /// <response code="200">User was successfully authenticate.</response>
        /// <response code="401">Incoming model credentials isn't valid.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpPost("login")]
        [ModelValidationFilter]
        [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthorizationDto userAuthorizationDto)
        {
            var isUserValid = await RepositoryManager.AuthenticationRepository.ValidateUserAsync(userAuthorizationDto);

            if (!isUserValid)
                return Unauthorized(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Authenticate was failed. Try another user name or password",
                    Details = "There can be few reasons. " +
                        "User with entered data don't exists; " +
                        "entered data are invalid (misspell); " +
                        "entered data contains invalid syntax (validation rules were violated)."
                });

            var token = RepositoryManager.AuthenticationRepository.CreateToken(userAuthorizationDto);

            return Ok(token);
        }

        /// <summary>
        /// Registrate user in system.
        /// </summary>
        /// <param name="userRegistrationDto"></param>
        /// <returns>If user already exists in system return BadRequest status with error model, if don't exists return Created status.</returns>
        /// <response code="200">User was successfully registered.</response>
        /// <response code="400">Incoming model already exists in system.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpPost("registration")]
        [ModelValidationFilter]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registrate([FromBody] UserRegistrationDto userRegistrationDto)
        {
            var isUserExisted = RepositoryManager.AuthenticationRepository.IsUserExisted(userRegistrationDto);

            if (isUserExisted)
                return Unauthorized(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "User already exists.",
                    Details = new StackTrace().ToString()
                });

            var userId = await RepositoryManager.AuthenticationRepository.RegistrateUserAsync(userRegistrationDto);

            return Created($"api/userCredentials/{userId}", userId);
        }

        /// <summary>
        /// Purge databse after tests.
        /// </summary>
        /// <param name="userCredentialsId">Deletion user credential id.</param>
        /// <returns></returns>
        /// <response code="204">User was successfully deleted.</response>
        /// <response code="404">User credential with given id not found.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpDelete("purge/{userCredentialsId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Purge(string userCredentialsId)
        {
            var userCredentials = await RepositoryManager.UserCredentialRepository.GetUserCredentialAsync(userCredentialsId);

            if (userCredentials == null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User don't exists.",
                    Details = new StackTrace().ToString()
                });

            var userAccount = await RepositoryManager.UserCredentialRepository.GetUserCredentialAccountAsync(userCredentialsId);
            await RepositoryManager.UserAccountRepository.DeleteAccountAsync(userAccount);

            await RepositoryManager.UserCredentialRepository.DeleteUserCredentialAsync(userCredentials);

            return NoContent();
        }
    }
}
