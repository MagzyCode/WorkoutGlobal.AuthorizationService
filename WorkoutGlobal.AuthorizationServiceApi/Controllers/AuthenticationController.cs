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
        private IAuthenticationRepository _authenticationRepository;

        /// <summary>
        /// Ctor for authentication controller.
        /// </summary>
        /// <param name="authenticationRepository">Repository authentication instance.</param>
        public AuthenticationController(IAuthenticationRepository authenticationRepository)
        {
            AuthenticationRepository = authenticationRepository;
        }

        /// <summary>
        /// Authentication repository property.
        /// </summary>
        public IAuthenticationRepository AuthenticationRepository
        {
            get => _authenticationRepository;
            private set => _authenticationRepository = value ?? throw new NullReferenceException(nameof(value));
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
            var isUserExisted = AuthenticationRepository.IsUserExisted(userRegistrationDto);

            if (isUserExisted)
                return Unauthorized(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "User already exists.",
                    Details = new StackTrace().ToString()
                });

            var userId = await AuthenticationRepository.RegistrateUserAsync(userRegistrationDto);

            return Created($"api/userCredentials/{userId}", userId);
        }

        

    }
}
