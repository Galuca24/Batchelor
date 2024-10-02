using Ergo.Api.Controllers;
using Licenta.API.Services;
using Licenta.Application.Features.Book;
using Licenta.Application.Features.Book.Commands.CreateBook;
using Licenta.Application.Features.Book.Commands.DeleteBook;
using Licenta.Application.Features.Book.Commands.ReturnBook;
using Licenta.Application.Features.Book.Commands.UpdateBook;
using Licenta.Application.Features.Book.Commands.UpdateStatusBook;
using Licenta.Application.Features.Book.Queries.GetAll;
using Licenta.Application.Features.Book.Queries.GetAvaibleBooks;
using Licenta.Application.Features.Book.Queries.GetBookById;
using Licenta.Application.Features.Book.Queries.GetMissingBooks;
using Licenta.Application.Features.Book.Queries.GetMostBorrowedBooksInLast7Days;
using Licenta.Application.Features.Book.Queries.GetRecomandations;
using Licenta.Application.Features.Book.Queries.GetReturnedBooksCount;
using Licenta.Application.Features.Book.Queries.GetReturnedBooksCountByUser;
using Licenta.Application.Features.Book.Queries.GetReturnedOverdue.Licenta.Application.Features.Loans.Queries.GetOverdueBooks;
using Licenta.Application.Features.Book.Queries.GetTimesBorrowed;
using Licenta.Application.Features.Book.Queries.SearchBooks;
using Licenta.Application.Features.Checkouts.Queries.GetOverdueCheckouts;
using Licenta.Application.Features.Checkouts.Queries.GetOverdueCheckoutsByUserId;
using Licenta.Application.Features.Checkouts.Queries.GetRecentCheckouts;
using Licenta.Application.Features.Checkouts.Queries.GetReturnsInTheLastSevenDays;
using Licenta.Application.Features.Loans.Commands.LoanBookCommand;
using Licenta.Application.Features.Loans.Queries.GetActiveLoansByUser;
using Licenta.Application.Features.Loans.Queries.GetAllLoansByUser;
using Licenta.Application.Features.Loans.Queries.GetCurrentLoansInLastSevenDays;
using Licenta.Application.Features.Loans.Queries.GetLoanedBooks;
using Licenta.Application.Features.Loans.Queries.GetLoanedBooksCount;
using Licenta.Application.Features.Loans.Queries.GetRemainingTime;
using Licenta.Application.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BooksController : ApiControllerBase
    {
        private readonly IGoogleBooksService _googleBooksService;

        public BooksController(IGoogleBooksService googleBooksService)
        {
            _googleBooksService = googleBooksService;
        }


        [HttpGet("get-recomandations-by-user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecomandationsByUser(Guid userId)
        {
            var query = new GetBookRecommendationsQuery(userId);
            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("GetMostBorrowedBooksInLast7Days")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMostBorrowedBooksInLast7Days()
        {
            var query = new GetMostBorrowedBooksInLast7DaysQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetOverdueBooksByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOverdueBooksByUserId(Guid userId)
        {
            var query = new GetOverdueCheckoutsByUserIdQuery { UserId = userId };
            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("GetTimesBorrowed/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimesBorrowed(Guid id)
        {
            var query = new GetTimesBorrowedQuery { BookId = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetBookById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var query = new GetBookByIdQuery { BookId = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("SearchBooks/{searchValue}")]
        public async Task<IActionResult> SearchBooks(string searchValue)
        {
            var query = new SearchBooksQuery { SearchValue = searchValue };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetOverdueCheckouts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOverdueCheckouts()
        {
            var query = new GetOverdueCheckoutsQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("recent-checkouts")]
        public async Task<IActionResult> GetRecentCheckouts()
        {
            var result = await Mediator.Send(new GetRecentCheckoutsQuery());
            return Ok(result);
        }

        [HttpGet("GetReturnsInLastSevenDays")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReturnsInLastSevenDays()
        {
            var query = new GetReturnsInLastSevenDaysQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetCurrentLoansInLastSevenDays")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentLoansInLastSevenDays()
        {
            var query = new GetCurrentLoansQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }



        [HttpGet("GetReturnedOverdueBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReturnedOverdueBooks()
        {
            var query = new GetReturnedOverdueBooksQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("remaining-time/{bookId}")]
        public async Task<IActionResult> GetRemainingTime(Guid bookId)
        {
            var query = new GetRemainingTimeQuery { BookId = bookId };
            var result = await Mediator.Send(query);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetMissingBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMissingBooks()
        {
            var query = new GetMissingBooksQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("GetAvailableBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailableBooks()
        {
            var query = new GetAvailableBooksQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("LoanedBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLoanedBooks()
        {
            var query = new GetLoanedBooksQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("ReturnedBooksCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReturnedBooksCount([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var query = new GetReturnedBooksCountQuery
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetReturnedBooksByUserCount/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReturnedBooksByUserCount(Guid id)
        {
            var query = new GetReturnedBooksCountByUserQuery(id);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAllBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetActiveLoansByUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveLoansByUser(Guid id)
        {
            var query = new GetActiveLoansByUserQuery { UserId = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetUserLoans/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserLoans(Guid id)
        {
            var query = new GetUserLoansQuery { UserId = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetLoanedBooksCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLoanedBooksCount()
        {
            var query = new GetLoanedBooksCountQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }


        

        [HttpGet("SearchGoogleBooks")]
        public async Task<IActionResult> SearchGoogleBooks(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query-ul nu poate fi gol.");
            }

            try
            {
                var result = await _googleBooksService.SearchGoogleBooks(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare la comunicarea cu Google Books API.");
            }
        }



        [HttpPost("AddBook")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBooks(CreateBookCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost("ReturnBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReturnBook(ReturnBookCommand returnBookDto)
        {
            var command = new ReturnBookCommand { BookId = returnBookDto.BookId, UserId = returnBookDto.UserId };
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost("LoanBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LoanBook(LoanBookCommand loanBookDto)
        {
            var command = new LoanBookCommand { BookId = loanBookDto.BookId, UserId = loanBookDto.UserId };
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBook(Guid bookId, UpdateBookDto bookDto)
        {
            var command = new UpdateBookCommand
            {
                BookId = bookId,
                Book = bookDto
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpPut("UpdateBookStatus/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBookStatus(Guid id, UpdateBookStatusDto updateBookStatusDto)
        {
            var command = new UpdateStatusBookCommand { BookId = id, Status = updateBookStatusDto.BookStatus };
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }




        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var command = new DeleteBookCommand { BookId = id };
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }



        
    }
}


