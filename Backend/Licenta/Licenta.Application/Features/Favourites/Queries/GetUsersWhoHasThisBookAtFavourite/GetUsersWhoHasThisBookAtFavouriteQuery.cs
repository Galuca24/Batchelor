using Licenta.Application.Features.Loans.Queries;
using Licenta.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites.Queries.GetUsersWhoHasThisBookAtFavourite
{
    public class GetUsersWhoHasThisBookAtFavouriteQuery : IRequest<List<Guid>>
    {
        public Guid BookId { get; set; }
    }
}
