using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkoutGlobal.AuthorizationServiceApi.Filters;
using WorkoutGlobal.AuthorizationServiceApi.Models;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.Repositories;

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
        /// <summary>
        /// Ctor for authentication controller.
        /// </summary>
        /// <param name="authenticationRepository">Authentication repository instance.</param>
        /// <param name="userCredentialRepository">User credentual repository instance.</param>
        /// <param name="userAccountRepository">User account repository instance.</param>
        public AuthenticationController(
            IAuthenticationRepository authenticationRepository,
            IUserCredentialRepository userCredentialRepository,
            IUserAccountRepository userAccountRepository)
        {
            AuthenticationRepository = authenticationRepository;
            CredentialRepository = userCredentialRepository;
            AccountRepository = userAccountRepository;
        }

        /// <summary>
        /// Authentication repository instance.
        /// </summary>
        public IAuthenticationRepository AuthenticationRepository { get; private set; }

        /// <summary>
        /// Authentication repository instance.
        /// </summary>
        public IUserCredentialRepository CredentialRepository { get; private set; }

        /// <summary>
        /// Authentication repository instance.
        /// </summary>
        public IUserAccountRepository AccountRepository { get; private set; }

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
            var isUserValid = await AuthenticationRepository.ValidateUserAsync(userAuthorizationDto);

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

            var token = AuthenticationRepository.CreateToken(userAuthorizationDto);
            var (refreshToken, expirationTime) = await AuthenticationRepository.RegisterRefreshToken(userAuthorizationDto.UserName);

            return Ok(new
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                RefreshTokenExpirationTime = expirationTime
            });
        }

        /// <summary>
        /// Registrate user in system.
        /// </summary>
        /// <param name="userRegistrationDto"></param>
        /// <returns>If user already exists in system return BadRequest status with error model, if don't exists return Created status.</returns>
        /// <response code="201">User was successfully registered.</response>
        /// <response code="400">Incoming model already exists in system.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpPost("registration")]
        [ModelValidationFilter]
        [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registrate([FromBody] UserRegistrationDto userRegistrationDto)
        {
            var isUserExisted = AuthenticationRepository.IsUserExisted(userRegistrationDto);

            if (isUserExisted)
                return Unauthorized(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "User already exists.",
                    Details = new StackTrace().ToString()
                });

            var userId = await AuthenticationRepository.RegistrateUserAsync(userRegistrationDto);
            await AuthenticationRepository.RegisterRefreshToken(userRegistrationDto.UserName);

            return Created($"api/userCredentials/{userId}", userId);
        }

        /// <summary>
        /// Refresh token for continue working with private resourses.
        /// </summary>
        /// <param name="userCredentialsId">User credential id.</param>
        /// <returns>Refresh token and it's expiration time.</returns>
        /// <response code="201">User was successfully registered.</response>
        /// <response code="400">Incoming model already exists in system.</response>
        /// <response code="404">User credential doesn't find by given id.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpPost("refresh/{userCredentialsId}")]
        [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh(string userCredentialsId)
        {
            if (string.IsNullOrEmpty(userCredentialsId))
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "User id not valid.",
                    Details = "Incoming id is null or empty."
                });

            var userCredential = await CredentialRepository.GetUserCredentialAsync(userCredentialsId);

            if (userCredential is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User don't exists.",
                    Details = "There is no user with given id."
                });

            var (refreshToken, expirationTime) = await AuthenticationRepository.RegisterRefreshToken(userCredential.UserName);

            return Ok(new
            {
                RefreshToken = refreshToken,
                RefreshTokenExpirationTime = expirationTime
            });
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
            var userCredentials = await CredentialRepository.GetUserCredentialAsync(userCredentialsId);

            if (userCredentials is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User don't exists.",
                    Details = new StackTrace().ToString()
                });

            var userAccount = await CredentialRepository.GetUserCredentialAccountAsync(userCredentialsId);
            await AccountRepository.DeleteAccountAsync(userAccount.Id);

            await CredentialRepository.DeleteUserCredentialAsync(userCredentials);

            return NoContent();
        }
    }
}
