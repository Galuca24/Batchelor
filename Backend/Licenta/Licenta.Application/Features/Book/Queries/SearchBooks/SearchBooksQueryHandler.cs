using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.SearchBooks
{
    public class SearchBooksQueryHandler : IRequestHandler<SearchBooksQuery, SearchBooksQueryResponse>
    {
        private readonly IBookRepository bookRepository;

        public SearchBooksQueryHandler(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<SearchBooksQueryResponse> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
        {
            var allBooks = await bookRepository.GetAllAsync();
            if (!allBooks.IsSuccess)
            {
                return new SearchBooksQueryResponse { Success = false, Message = allBooks.Error };
            }

            var books = allBooks.Value.Where(b =>
                (!string.IsNullOrWhiteSpace(b.Title) && b.Title.ToLower().Contains(request.SearchValue.ToLower())) ||
                (!string.IsNullOrWhiteSpace(b.Author) && b.Author.ToLower().Contains(request.SearchValue.ToLower())) ||
                (!string.IsNullOrWhiteSpace(b.Genre) && b.Genre.ToLower().Contains(request.SearchValue.ToLower()))
                                         ).ToArray();


            return new SearchBooksQueryResponse
            {
                Success = true,
                Books = books.Select(b => new SearchBookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                    Genre = b.Genre,
                    ISBN = b.ISBN,
                    Status = b.BookStatus
                }).Take(25).ToArray()
            };
        }
    }
}
