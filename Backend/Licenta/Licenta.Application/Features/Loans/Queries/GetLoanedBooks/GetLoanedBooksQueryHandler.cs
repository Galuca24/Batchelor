using Licenta.Application.Features.Book;
using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetLoanedBooks
{
    public class GetLoanedBooksQueryHandler : IRequestHandler<GetLoanedBooksQuery, List<BookDto>>
    {
        private readonly IBookRepository _bookRepository;

        public GetLoanedBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDto>> Handle(GetLoanedBooksQuery request, CancellationToken cancellationToken)
        {
            var result = await _bookRepository.GetAllAsync();

            if (!result.IsSuccess)
            {
                return new List<BookDto>(); 
            }

            var loanedBooks = result.Value
                .Where(b => b.BookStatus == BookStatus.Loaned)
                .Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    ISBN = b.ISBN,
                    Description = b.Description,
                    BookStatus = b.BookStatus
                })
                .ToList();

            return loanedBooks;
        }
    }
}
