using Licenta.Application.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites.Queries.GetFavouriteByUser
{
    public class GetFavouritesByUserQueryHandler : IRequestHandler<GetFavouriteByUserQuery, List<FavouriteBookDto>>
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IBookRepository _bookRepository;

        public GetFavouritesByUserQueryHandler(IFavouriteRepository favouriteRepository, IBookRepository bookRepository)
        {
            _favouriteRepository = favouriteRepository;
            _bookRepository = bookRepository;
        }

        public async Task<List<FavouriteBookDto>> Handle(GetFavouriteByUserQuery request, CancellationToken cancellationToken)
        {
            var favouritesResult = await _favouriteRepository.GetFavouritesByUserIdAsync(request.UserId);
            if (!favouritesResult.IsSuccess || favouritesResult.Value == null)
            {
                throw new Exception(favouritesResult.Error); // Consider a more specific exception or handling strategy
            }

            var bookIds = favouritesResult.Value
                          .Where(f => f.BookId.HasValue)
                          .Select(f => f.BookId.Value)
                          .ToList();

            if (!bookIds.Any())
            {
                return new List<FavouriteBookDto>(); 
            }

            var booksResult = await _bookRepository.FindByIdsAsync(bookIds);
            if (!booksResult.IsSuccess || booksResult.Value == null)
            {
                throw new Exception(booksResult.Error); 
            }

            var books = booksResult.Value; 
            return books.Select(book => new FavouriteBookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author ,
                Genre = book.Genre, 
                ImageUrl = book.ImageUrl,
                Description = book.Description,
                Status = book.BookStatus
            }).ToList();
        }

    }
}
