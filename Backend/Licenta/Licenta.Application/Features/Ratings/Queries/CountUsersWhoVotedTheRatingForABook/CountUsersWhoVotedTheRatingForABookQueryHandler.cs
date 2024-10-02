using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Ratings.Queries.CountUsersWhoVotedTheRatingForABook
{
    public class CountUsersWhoVotedTheRatingForABookQueryHandler : IRequestHandler<CountUsersWhoVotedTheRatingForABookQuery, int>
    {
        private readonly IRatingRepository _ratingRepository;

        public CountUsersWhoVotedTheRatingForABookQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<int> Handle(CountUsersWhoVotedTheRatingForABookQuery request, CancellationToken cancellationToken)
        {
            return await _ratingRepository.CountUsersWhoVotedTheRatingForABook(request.BookId);
        }

    }
}
