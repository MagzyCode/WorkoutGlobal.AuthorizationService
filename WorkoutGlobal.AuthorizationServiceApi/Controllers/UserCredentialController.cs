using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Enums;
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
        /// <summary>
        /// Ctor for user credential controller.
        /// </summary>
        /// <param name="userCredentialRepository">Credential repository.</param>
        /// <param name="mapper">Auto mapper.</param>
        public UserCredentialController(
            IUserCredentialRepository userCredentialRepository,
            IMapper mapper)
        {
            CredentialRepository = userCredentialRepository;
            Mapper = mapper;
        }

        /// <summary>
        /// Credential repository.
        /// </summary>
        public IUserCredentialRepository CredentialRepository { get; private set; }

        /// <summary>
        /// Auto mapping helper.
        /// </summary>
        public IMapper Mapper { get; private set; }

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
            var userCredentials = await CredentialRepository.GetAllUserCredentialsAsync();

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
            var userCredential = await CredentialRepository.GetUserCredentialAsync(userCredentialId);

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
            var userCredential = await CredentialRepository.GetUserCredentialAsync(userCredentialId);

            if (userCredential is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User don't exists.",
                    Details = new StackTrace().ToString()
                });

            var roles = await CredentialRepository.GetUserCredentialRolesAsync(userCredentialId);

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
            var userCredential = await CredentialRepository.GetUserCredentialAsync(userCredentialId);

            if (userCredential is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User don't exists.",
                    Details = new StackTrace().ToString()
                });

            var account = await CredentialRepository.GetUserCredentialAccountAsync(userCredentialId);

            var accountDto = Mapper.Map<UserAccountDto>(account);

            return Ok(accountDto);
        }


        /// <summary>
        /// Delete account.
        /// </summary>
        /// <param name="credentialsId">Deletion id.</param>
        /// <param name="deleteType">Deletion type.</param>
        /// <returns></returns>
        [ProducesResponseType(type: typeof(NoContentResult), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        [HttpDelete("{credentialsId}")]
        public async Task<IActionResult> DeleteUserCredential(string credentialsId, DeleteType deleteType)
        {
            if (string.IsNullOrEmpty(credentialsId))
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var user = await CredentialRepository.GetUserCredentialAsync(credentialsId);

            if (user is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "There is no user with such id.",
                    Details = new StackTrace().ToString()
                });

            var type = Enum.IsDefined(deleteType)
                ? deleteType
                : DeleteType.Hard;

            await CredentialRepository.DeleteUserCredentialAsync(user, type);

            return NoContent();
        }
    }
}
