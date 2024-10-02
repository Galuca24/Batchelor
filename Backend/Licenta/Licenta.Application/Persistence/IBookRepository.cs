using Licenta.Domain.Common;
using Licenta.Domain.Entities;

namespace Licenta.Application.Persistence
{
    public interface IBookRepository: IAsyncRepository<Book>
    {
        Task<bool> BookExists(Guid bookId);
        Task<int> CountBooksByStatusAsync(BookStatus status);
        Task<Result<List<Book>>> FindByIdsAsync(List<Guid> ids);

        Task<List<Book>> GetAvailableBooksAsync();

        Task<List<Book>> GetBooksByAuthor(string author);

        Task<List<Book>> GetBooksByGenre(string genre);


    }
}
