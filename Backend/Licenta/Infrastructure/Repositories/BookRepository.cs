using Licenta.Application.Persistence;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookRepository:BaseRepository<Book>, IBookRepository
    {
        public BookRepository(LicentaContext context) : base(context)
        {
        }

        public async Task<List<Book>> GetBooksByAuthor(string author)
        {
            return await _context.Books.Where(b => b.Author == author).ToListAsync();
        }

        public async Task<bool> BookExists(Guid bookId)
        {
            return await _context.Books.AnyAsync(b => b.BookId == bookId);
        }

        public async Task<List<Book>> GetBooksByGenre(string genre)
        {
            return await _context.Books.Where(b => b.Genre == genre).ToListAsync();
        }


        public async Task<int> CountBooksByStatusAsync(BookStatus status)
        {
            return await _context.Books.CountAsync(b => b.BookStatus == status);
        }

        public async Task<Result<List<Book>>> FindByIdsAsync(List<Guid> ids)
        {
            var books = await _context.Books
                                      .Where(b => ids.Contains(b.BookId))
                                      .ToListAsync();
            if (books == null || books.Count != ids.Count)
            {
                return Result<List<Book>>.Failure("Not all books were found.");
            }
            return Result<List<Book>>.Success(books);
        }
        public async Task<List<Book>> GetAvailableBooksAsync()
        {
            return await _context.Set<Book>()
                                 .Where(b => b.BookStatus == BookStatus.Available)
                                 .ToListAsync();
        }


    }
}
