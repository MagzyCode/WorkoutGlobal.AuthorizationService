using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Models;

namespace WorkoutGlobal.AuthorizationServiceApi.Controllers
{
    /// <summary>
    /// User accounts controller.
    /// </summary>
    [Route("api/accounts")]
    [ApiController]
    [Produces("application/json")]
    public class UserAccountController : ControllerBase
    {
        private IRepositoryManager _repositoryManager;
        private IMapper _mapper;

        /// <summary>
        /// Ctor for user accounts controller.
        /// </summary>
        /// <param name="repositoryManager">Repository manager.</param>
        /// <param name="mapper">Auto mapper.</param>
        public UserAccountController(
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
        /// Get user account by id.
        /// </summary>
        /// <param name="accountId">User account id.</param>
        /// <returns>Return find user account.</returns>
        /// <response code="200">User was successfully get.</response>
        /// <response code="404">User credential with given id not found.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpGet("{accountId}")]
        [ProducesResponseType(type: typeof(UserAccountDto), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserAccount(Guid accountId)
        {
            var user = await RepositoryManager.UserAccountRepository.GetAccountAsync(accountId);

            if (user is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "There is no user with such id.",
                    Details = new StackTrace().ToString()
                });

            var userAccountDto = Mapper.Map<UserAccountDto>(user);

            return Ok(userAccountDto);
        }
    }
}
