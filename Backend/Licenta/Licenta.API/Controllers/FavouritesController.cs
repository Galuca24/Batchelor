using Ergo.Api.Controllers;
using Licenta.Application.Features.Favourites.Commands.AddFavourite;
using Licenta.Application.Features.Favourites.Commands.RemoveBookFromFavourite;
using Licenta.Application.Features.Favourites.Queries.GetFavouriteByUser;
using Licenta.Application.Features.Favourites.Queries.GetUsersWhoHasThisBookAtFavourite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FavouritesController : ApiControllerBase
    {
        

        [HttpGet("get-users-who-has-this-book-at-favourite/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersWhoHasThisBookAtFavourite(Guid bookId)
        {
            var response = await Mediator.Send(new GetUsersWhoHasThisBookAtFavouriteQuery { BookId = bookId });
            return Ok(response);
        }

        [HttpGet("get-favourites-by-user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavouritesByUser(Guid userId)
        {
            var response = await Mediator.Send(new GetFavouriteByUserQuery { UserId = userId });
            return Ok(response);
        }

        [HttpPost("add-book-to-favourite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddFavourite([FromBody] AddFavouriteCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("remove-book-from-favourite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveBookFromFavourite([FromBody] RemoveBookFromFavouriteCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
