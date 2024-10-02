using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        public RatingRepository(LicentaContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Rating>> GetRatingsGivenByUserId(Guid userId)
        {
            return await _context.Ratings
                               .Where(f => f.UserId == userId)
                               .ToListAsync();
        }

        public async Task<int> CountUsersWhoVotedTheRatingForABook(Guid bookId)
        {
            return await _context.Ratings
                               .Where(f => f.BookId == bookId)
                               .CountAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByBookIdAsync(Guid bookId)
        {
            return await _context.Ratings
                               .Where(f => f.BookId == bookId)
                               .ToListAsync();
        }

        public async Task<Rating> FindByBookAndUserIdAsync(Guid bookId, Guid userId)
        {
            return await _context.Ratings
                               .Where(f => f.UserId == userId && f.BookId == bookId)
                               .FirstOrDefaultAsync();
        }
    }
    
}
