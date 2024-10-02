using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Ratings.Queries.GetRatingsGivenByUserId
{
    public class GetRatingsGivenByUserIdQueryHandler : IRequestHandler<GetRatingsGivenByUserIdQuery, List<RatingDto>>
    {
        private readonly IRatingRepository _ratingRepository;

        public GetRatingsGivenByUserIdQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<List<RatingDto>> Handle(GetRatingsGivenByUserIdQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _ratingRepository.GetRatingsGivenByUserId(request.UserId);
            return ratings.Select(rating => new RatingDto
            {
                RatingId = rating.RatingId,
                BookId = rating.BookId,
                UserId = rating.UserId,
                Value = rating.Value
            }).ToList();
        }

    }
}
