using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
    public interface IRatingRepository : IAsyncRepository<Rating>
    {
        Task<IEnumerable<Rating>> GetRatingsGivenByUserId(Guid userId);
        Task<Rating> FindByBookAndUserIdAsync(Guid bookId, Guid userId);

        Task<IEnumerable<Rating>> GetRatingsByBookIdAsync(Guid bookId);

        Task<int> CountUsersWhoVotedTheRatingForABook(Guid bookId);
    }
}
