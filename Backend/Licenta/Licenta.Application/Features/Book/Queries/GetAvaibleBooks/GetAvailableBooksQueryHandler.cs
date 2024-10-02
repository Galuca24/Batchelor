using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetAvaibleBooks
{
    public class GetAvailableBooksQueryHandler : IRequestHandler<GetAvailableBooksQuery, List<BookDto>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAvailableBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDto>> Handle(GetAvailableBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAvailableBooksAsync();
            return books.Select(b => new BookDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                ISBN = b.ISBN,
                Description = b.Description,
                BookStatus = b.BookStatus
            }).ToList();
        }
    }
}