using Licenta.Application.Persistence;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public  class FavouriteRepository : BaseRepository<Favourite>, IFavouriteRepository
    {
        public FavouriteRepository(LicentaContext context) : base(context)
        {
        }

  

        public async Task<List<Guid>> GetUserIdsWithBookInFavouritesAsync(Guid bookId)
        {
            var userIdsNullable = await _context.Favourites
                .Where(f => f.BookId == bookId)
                .Select(f => f.UserId)
                .Distinct()
                .ToListAsync();

            var userIds = userIdsNullable
                .Where(userId => userId.HasValue)
                .Select(userId => userId.Value)
                .ToList();

            return userIds;
        }



        public async Task<Result<Favourite>> DeleteByUserIdAndBookIdAsync(Guid userId, Guid bookId)
        {
            var favourite = await _context.Favourites
                .Where(f => f.UserId == userId && f.BookId == bookId)
                .FirstOrDefaultAsync();

            if (favourite == null)
            {
                return Result<Favourite>.Failure($"No favourite found for user with id {userId} and book with id {bookId}");
            }

            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();

            return Result<Favourite>.Success(favourite);
        }

        public async Task<Result<List<Favourite>>> GetFavouritesByUserIdAsync(Guid userId)
        {
            var favourites = await _context.Favourites
                .Where(f => f.UserId == userId)
                .ToListAsync();

            if (favourites == null)
            {
                return Result<List<Favourite>>.Failure($"No favourites found for user with id {userId}");
            }

            return Result<List<Favourite>>.Success(favourites);
        }

        public async Task<Favourite> FindByUserIdAndBookIdAsync(Guid? userId, Guid? bookId)
        {
            return await _context.Favourites
                    .Where(f => f.UserId == userId && f.BookId == bookId)
                    .FirstOrDefaultAsync();
        }
    }
    
}
