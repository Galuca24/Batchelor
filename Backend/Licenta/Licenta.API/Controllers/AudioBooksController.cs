using Ergo.Api.Controllers;
using Licenta.API.Services;
using Licenta.Application.Features.AudioBook.Commands.CreateAudioBook;
using Licenta.Application.Features.AudioBook.Commands.DeleteAudioBook;
using Licenta.Application.Features.AudioBook.Commands.UpdateAudioBook;
using Licenta.Application.Features.AudioBook.Queries.GetAll;
using Licenta.Application.Features.AudioBook.Queries.SearchAudioBooks;
using Licenta.Application.Features.Book.Commands.CreateBook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AudioBooksController : ApiControllerBase
    {
        private readonly ILibrivoxService _librivoxService;


        public AudioBooksController(ILibrivoxService librivoxService)
        {
            _librivoxService = librivoxService;
        }
        
        [HttpGet("get-all-audioBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAudioBooks()
        {
            var query = new GetAllQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("Search-Audio-Books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAudioBooks(string query)
        {
            var searchAudioBookQuery = new SearchAudioBookQuery { SearchValue = query };
            var result = await Mediator.Send(searchAudioBookQuery);
            return Ok(result);
        }


        [HttpGet("Search-Librivox-Books")]
        public async Task<IActionResult> SearchLibrivoxBooks(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query-ul nu poate fi gol.");
            }

            try
            {
                var result = await _librivoxService.SearchLibrivoxBooksAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"A apărut o eroare la comunicarea cu Librivox API: {ex.Message}");
            }
        }

        

        [HttpPost("Add-Audio-Book")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBooks(CreateAudioBookCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Update-Audio-Book")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAudioBook(Guid audioBookId,UpdateAudioBookDto audioBookDto)
        {
            var command = new UpdateAudioBookCommand { AudioBookId = audioBookId, AudioBook = audioBookDto };
            var result = await Mediator.Send(command);
            return Ok(result);
        }


        [HttpDelete("Delete-Audio-Book")]
        [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> DeleteAudioBook(DeleteAudioBookCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
