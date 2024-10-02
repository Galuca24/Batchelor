using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
    public interface IFavouriteRepository : IAsyncRepository<Favourite>
    {

        Task<List<Guid>> GetUserIdsWithBookInFavouritesAsync(Guid bookId);

        Task<Favourite> FindByUserIdAndBookIdAsync(Guid? userId, Guid? bookId);
        Task<Result<List<Favourite>>> GetFavouritesByUserIdAsync(Guid userId);

        Task<Result<Favourite>> DeleteByUserIdAndBookIdAsync(Guid userId, Guid bookId);


    }
}
