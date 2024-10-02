using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Ratings.Queries.CountUsersWhoVotedTheRatingForABook
{
    public class CountUsersWhoVotedTheRatingForABookQuery : IRequest<int>
    {
        public Guid BookId { get; set; }
    }
}
