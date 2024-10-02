using Licenta.Domain.Common;
using Licenta.Domain.Entities;

namespace Licenta.Application.Persistence
{
    public interface IReviewRepository: IAsyncRepository<Review>
    {
        Task<Result<IReadOnlyList<Review>>> GetReviewsByBookIdAsync(Guid bookId);
        Task<Result<List<Review>>> GetReviewsByAudioBookIdAsync(Guid audioBookId);


    }
}
