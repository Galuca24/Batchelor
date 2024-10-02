using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookByIdQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _bookRepository.FindByIdAsync(request.BookId);

            if (!result.IsSuccess || result.Value == null)
                throw new Exception("Book not found"); // Consider using a more specific exception

            var book = result.Value;
            return new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                ISBN = book.ISBN,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                NumberOfCopies = book.NumberOfCopies,
                BookStatus = book.BookStatus
            };
        }
    }
}
