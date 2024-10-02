using Ergo.Api.Controllers;
using Licenta.Application.Features.Loans.Queries.GetMostBorrowedBooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ApiControllerBase
    {

        [HttpGet("GetMostBorrowedBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MostBorrowedBookDto>>> GetMostBorrowedBooks()
        {
           var query = new GetMostBorrowedBooksQuery();
           var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
