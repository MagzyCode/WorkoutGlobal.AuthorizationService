using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Extensions;
using WorkoutGlobal.AuthorizationServiceApi.Models;
using WorkoutGlobal.Shared.Messages;

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
        /// <summary>
        /// Ctor for user accounts controller.
        /// </summary>
        /// <param name="userAccountRepository">Account repository.</param>
        /// <param name="mapper">Auto mapper.</param>
        /// <param name="publisher">Publish service.</param>
        public UserAccountController(
            IUserAccountRepository userAccountRepository,
            IMapper mapper,
            IPublishEndpoint publisher)
        {
            AccountRepository = userAccountRepository;
            Mapper = mapper;
            Publisher = publisher;
        }

        /// <summary>
        /// Account repository.
        /// </summary>
        public IUserAccountRepository AccountRepository { get; private set; }

        /// <summary>
        /// Auto mapping helper.
        /// </summary>
        public IMapper Mapper { get; private set; }

        /// <summary>
        /// Publish service.
        /// </summary>
        public IPublishEndpoint Publisher { get; private set; }

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
            if (accountId == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var user = await AccountRepository.GetAccountAsync(accountId);

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

        /// <summary>
        /// Update user account.
        /// </summary>
        /// <param name="id">User account id.</param>
        /// <param name="updationUserAccountDto">Updation model.</param>
        /// <returns></returns>
        [ProducesResponseType(type: typeof(NoContentResult), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAccount(Guid id, [FromBody] UpdationUserAccountDto updationUserAccountDto)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var user = await AccountRepository.GetAccountAsync(id, false);

            if (user is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "There is no user with such id.",
                    Details = new StackTrace().ToString()
                });

            var updationUser = Mapper.Map<UserAccount>(updationUserAccountDto);

            updationUser.Id = id;
            updationUser.DateOfRegistration = user.DateOfRegistration;
            updationUser.IsStatusVerify = user.IsStatusVerify;

            await AccountRepository.UpdateAccountAsync(updationUser);

            await Publisher.Publish<UpdateUserMessage>(
                    message: new(
                        UpdationId: id,
                        FirstName: updationUser.FirstName,
                        LastName: updationUser.LastName,
                        Patronymic: updationUser.Patronymic));

            return NoContent();
        }
    }
}
