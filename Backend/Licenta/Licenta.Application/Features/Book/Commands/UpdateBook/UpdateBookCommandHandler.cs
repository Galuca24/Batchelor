using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;

        public UpdateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var result = await _bookRepository.FindByIdAsync(request.BookId);

            if (!result.IsSuccess || result.Value == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            var book = result.Value;

            // Asigură-te că nu actualizezi cu valori null dacă DTO-ul nu le include
            book.Title = request.Book.Title ?? book.Title;
            book.Author = request.Book.Author ?? book.Author;
            book.ISBN = request.Book.ISBN ?? book.ISBN;
            book.Genre = request.Book.Genre ?? book.Genre;
            book.Description = request.Book.Description ?? book.Description;
            book.ImageUrl = request.Book.ImageUrl ?? book.ImageUrl;

            await _bookRepository.UpdateAsync(book);

            return new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Genre = book.Genre,
                Description = book.Description,
                ImageUrl = book.ImageUrl
            };
        }
    }

}

