using Ergo.Api.Controllers;
using Licenta.Application.Features.Memberships.Commands.ActivateMembership;
using Licenta.Application.Features.Memberships.Queries.GetMembershipStatusByUserId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MembershipsController : ApiControllerBase
    {

        [HttpGet("GetMembershipStatusByUserId")]
        public async Task<IActionResult> GetMembershipStatusByUserId([FromQuery] GetMembershipStatusByUserIdQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("ActivateMembership")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivateMembership([FromBody] ActivateMembershipCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

    }
}
