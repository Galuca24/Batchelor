using Ergo.Api.Controllers;
using Licenta.Application.Features.Reviews.Commands.CreateReview;
using Licenta.Application.Features.Reviews.Commands.CreateReviewForAudioBook;
using Licenta.Application.Features.Reviews.Commands.DeleteReview;
using Licenta.Application.Features.Reviews.Commands.UpdateReview;
using Licenta.Application.Features.Reviews.Queries.GetByAudioBookId;
using Licenta.Application.Features.Reviews.Queries.GetByBookId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController : ApiControllerBase
    {

        [HttpGet("getreviewsbyaudiobookid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewsByAudioBookId(Guid audioBookId)
        {
            var response = await Mediator.Send(new GetReviewByAudioBookIdQuery { AudioBookId = audioBookId });
            return Ok(response);
        }

        [HttpGet("GetReviewsByBookId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewsByBookId(Guid bookId)
        {
            var response = await Mediator.Send(new GetReviewByBookIdQuery { BookId = bookId });
            return Ok(response);
        }

        [HttpPost("Create-review-for-audio-book")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReviewForAudioBook([FromBody] CreateReviewForAudioBookCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("CreateReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }


        [HttpPut("UpdateReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }



        [HttpDelete("DeleteReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteReview([FromBody] DeleteCommentCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
