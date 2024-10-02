using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetRecomandations
{
    using Licenta.Application.Persistence;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetBookRecommendationsHandler : IRequestHandler<GetBookRecommendationsQuery, List<BookDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;

        public GetBookRecommendationsHandler(IBookRepository bookRepository, ILoanRepository loanRepository)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
        }

        public async Task<List<BookDto>> Handle(GetBookRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var loanResult = await _loanRepository.GetAllLoansByUserId(request.UserId);
            if (!loanResult.IsSuccess)
                return new List<BookDto>();  

            var loanedBookIds = loanResult.Value.Select(l => l.BookId).ToList();

            var loanedBooks = new List<BookDto>();
            foreach (var bookId in loanedBookIds)
            {
                var result = await _bookRepository.FindByIdAsync(bookId);
                if (result.IsSuccess && result.Value != null)
                {
                    var book = result.Value;
                    loanedBooks.Add(new BookDto
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

            var genreCounts = new Dictionary<string, int>();
            var authorCounts = new Dictionary<string, int>();
            foreach (var book in loanedBooks)
            {
                if (!genreCounts.ContainsKey(book.Genre))
                    genreCounts[book.Genre] = 0;
                genreCounts[book.Genre]++;

                if (!authorCounts.ContainsKey(book.Author))
                    authorCounts[book.Author] = 0;
                authorCounts[book.Author]++;
            }

            var topGenres = genreCounts.OrderByDescending(kvp => kvp.Value).Take(2).Select(kvp => kvp.Key).ToList();
            var topAuthors = authorCounts.OrderByDescending(kvp => kvp.Value).Take(2).Select(kvp => kvp.Key).ToList();

            var recommendedBooks = new List<BookDto>();

            foreach (var genre in topGenres)
            {
                var booksInGenre = await _bookRepository.GetBooksByGenre(genre);
                recommendedBooks.AddRange(booksInGenre.Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    ISBN = b.ISBN,
                    Description = b.Description,
                    BookStatus = b.BookStatus,
                    ImageUrl = b.ImageUrl,
                    NumberOfCopies = b.NumberOfCopies
                }));
            }

            foreach (var author in topAuthors)
            {
                var booksByAuthor = await _bookRepository.GetBooksByAuthor(author);
                recommendedBooks.AddRange(booksByAuthor.Where(b => !loanedBookIds.Contains(b.BookId) && !topGenres.Contains(b.Genre))
                                                       .Select(b => new BookDto
                                                       {
                                                           BookId = b.BookId,
                                                           Title = b.Title,
                                                           Author = b.Author,
                                                           Genre = b.Genre,
                                                           ISBN = b.ISBN,
                                                           Description = b.Description,
                                                           BookStatus = b.BookStatus,
                                                           ImageUrl = b.ImageUrl,
                                                           NumberOfCopies = b.NumberOfCopies
                                                       }));
                                                       
            }

            return recommendedBooks.Distinct().ToList();
        }




    }
}


