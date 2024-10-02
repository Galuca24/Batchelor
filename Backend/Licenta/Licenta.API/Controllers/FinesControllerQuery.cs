using Ergo.Api.Controllers;
using Licenta.Application.Features.Fines;
using Licenta.Application.Features.Fines.Commands.UpdateFineStatus;
using Licenta.Application.Features.Fines.Queries.GetAllFines;
using Licenta.Application.Features.Fines.Queries.GetFinesByUser;
using Licenta.Application.Features.Fines.Queries.GetTotalFinesAmount;
using Licenta.Application.Features.Fines.Queries.GetTotalFinesAmountByUser;
using Licenta.Application.Features.Fines.Queries.GetTotalUnpaidFinesAmountByUser;
using Licenta.Application.Features.Fines.Queries.GetUnpaidFinesByUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FinesController : ApiControllerBase
    {

        [HttpGet("GetAllFines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<FinesDto>>> GetAllFines()
        {
            return Ok(await Mediator.Send(new GetAllFinesQuery()));
        }

        [HttpGet("GetFinesByUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<FinesDto>>> GetFinesByUser(Guid id)
        {
            return Ok(await Mediator.Send(new GetFinesByUserQuery { UserId = id }));
        }

        [HttpGet("GetUnpaidFinesAmountByUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetUnpaidFinesAmountByUser(Guid id)
        {
            return Ok(await Mediator.Send(new GetTotalUnpaidFinesAmountQuery { UserId = id }));
        }

        [HttpGet("GetUnpaidFinesByUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UnpaidFineDto>>> GetUnpaidFinesByUser(Guid id)
        {
            return Ok(await Mediator.Send(new GetUnpaidFinesQuery { UserId = id }));
        }

        [HttpGet("GetTotalFinesAmount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetTotalFinesAmount()
        {
            return Ok(await Mediator.Send(new GetTotalFinesAmountQuery()));
        }

        [HttpGet("GetTotalFinesAmountByUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetTotalFinesAmountByUser(Guid id)
        {
            return Ok(await Mediator.Send(new GetTotalFinesAmountByUserQuery { UserId = id }));
        }

        [HttpPut("PayFine/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PayFine(Guid id)
        {
            return Ok(await Mediator.Send(new UpdateFineStatusCommand { FineId = id }));
        }
    }
}
