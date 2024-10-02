using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetAll
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDto>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var booksResult = await _bookRepository.GetAllAsync();
            var bookDtos = new List<BookDto>();

            if (booksResult.IsSuccess && booksResult.Value != null)
            {
                foreach (var book in booksResult.Value) 
                {
                    bookDtos.Add(new BookDto
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        Author = book.Author,
                        Genre = book.Genre,
                        ISBN = book.ISBN,
                        Description = book.Description,
                        BookStatus = book.BookStatus,
                        ImageUrl = book.ImageUrl,
                        NumberOfCopies = book.NumberOfCopies
                    });
                }
            }
            else
            {
                throw new Exception("Nu s-au putut obține cărțile din baza de date.");
             
            }

            return bookDtos;
        }
    }
}
