using Ergo.Api.Controllers;
using Licenta.Application.Features.Loans.Commands.DeleteUser;
using Licenta.Application.Features.Users.Commands.UpdateUser;
using Licenta.Application.Features.Users.Queries.GetAdminUsers;
using Licenta.Application.Features.Users.Queries.GetAll;
using Licenta.Application.Features.Users.Queries.GetByEmail;
using Licenta.Application.Features.Users.Queries.GetById;
using Licenta.Application.Features.Users.Queries.SearchUsers;
using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    public class UsersController: ApiControllerBase
    {
      
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The ids must be the same!");
            }
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand() { UserId = id };
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdminUsers()
        {
            var query = new GetAdminUsersQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

       
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllUsersQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("ByEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var query = new GetByEmailUserQuery { Email = email };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetByIdUserQuery { UserId = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("search/{searchValue}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchUsers(string searchValue)
        {
            var query = new SearchUsersQuery { SearchValue = searchValue };
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
