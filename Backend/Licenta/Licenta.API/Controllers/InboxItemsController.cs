using Ergo.Api.Controllers;
using Licenta.Application.Features.InboxItems.Commands.CreateInboxItem;
using Licenta.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using Licenta.Application.Features.InboxItems.Queries.GetByUserId;
using Licenta.Application.Features.InboxItems.Queries.GetUnreaByUserId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InboxItemsController : ApiControllerBase
    {

        [HttpGet("get-by-user-id/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetByUserIdQueryResponse>> GetByUserId(Guid userId)
        {
            var query = new GetByUserIdQuery() { UserId = userId };
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("get-unread-by-user-id/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<InboxItemDto>>> GetUnreadByUserId(Guid userId)
        {
            var query = new GetUnreadByUserIdQuery() { UserId = userId };
            return Ok(await Mediator.Send(query));
        }


        [HttpPost("send-notification")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateInboxItemCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPut("update-is-read/{inboxItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateIsRead(Guid inboxItemId)
        {
            var command = new UpdateInboxItemIsReadCommand() { InboxItemId = inboxItemId };
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return NoContent();
        }

    }
}
