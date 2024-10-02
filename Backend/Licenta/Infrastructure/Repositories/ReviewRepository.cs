
using Licenta.Application.Persistence;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReviewRepository: BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(LicentaContext context) : base(context)
        {

        }

        public async Task<Result<List<Review>>> GetReviewsByAudioBookIdAsync(Guid audioBookId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Where(r => r.AudioBookId == audioBookId)
                    .ToListAsync();

                return Result<List<Review>>.Success(reviews);
            }
            catch (Exception ex)
            {
                return Result<List<Review>>.Failure($"Error retrieving reviews: {ex.Message}");
            }
        }

        public virtual async Task<Result<IReadOnlyList<Review>>> GetReviewsByBookIdAsync(Guid bookId)
        {
            try
            {
                var reviews = await _context.Set<Review>()
                    .Where(r => r.BookId == bookId)
                    .AsNoTracking()
                    .ToListAsync();

                IReadOnlyList<Review> readOnlyReviews = reviews.Cast<Review>().ToList();

                return Result<IReadOnlyList<Review>>.Success(readOnlyReviews);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyList<Review>>.Failure($"An error occurred while fetching reviews: {ex.Message}");
            }
        }

    }
    
}
