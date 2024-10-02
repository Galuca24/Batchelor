using Licenta.Application.Features.Loans.Queries;
using Licenta.Application.Persistence;
using Licenta.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites.Queries.GetUsersWhoHasThisBookAtFavourite
{
    public class GetUsersWhoHasThisBookAtFavouriteHandler : IRequestHandler<GetUsersWhoHasThisBookAtFavouriteQuery, List<Guid>>
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public GetUsersWhoHasThisBookAtFavouriteHandler(IFavouriteRepository favouriteRepository  )
        {
            _favouriteRepository = favouriteRepository;
        }

        public async Task<List<Guid>> Handle(GetUsersWhoHasThisBookAtFavouriteQuery request, CancellationToken cancellationToken)
        {
            return await _favouriteRepository.GetUserIdsWithBookInFavouritesAsync(request.BookId);
        }
    }
}
