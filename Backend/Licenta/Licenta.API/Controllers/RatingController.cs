using Ergo.Api.Controllers;
using Licenta.Application.Features.Ratings.Commands.GiveRatingToBook;
using Licenta.Application.Features.Ratings.Queries.CountUsersWhoVotedTheRatingForABook;
using Licenta.Application.Features.Ratings.Queries.GetBookRatingsAverage;
using Licenta.Application.Features.Ratings.Queries.GetRatingsGivenByUserId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RatingController : ApiControllerBase
    {

        [HttpGet("GetRatingsGivenByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRatingsGivenByUserId(Guid id)
        {
            var response = await Mediator.Send(new GetRatingsGivenByUserIdQuery { UserId = id });
            return Ok(response);
        }

        [HttpGet("GetVotesCount/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVotesCount(Guid id)
        {
            var response = await Mediator.Send(new CountUsersWhoVotedTheRatingForABookQuery { BookId = id });
            return Ok(response);
        }

        [HttpGet("GetBookRatingsAverage/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookRatingsAverage(Guid id)
        {
            var response = await Mediator.Send(new GetBookRatingsAverageQuery { BookId = id });
            return Ok(response);
        }

        [HttpPost("GiveRatingToBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GiveRatingToBook([FromBody] GiveRatingToBookCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
