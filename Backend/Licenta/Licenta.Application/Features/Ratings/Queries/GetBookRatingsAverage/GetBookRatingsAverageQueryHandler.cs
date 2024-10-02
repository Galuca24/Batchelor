using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Ratings.Queries.GetBookRatingsAverage
{
    public class GetBookRatingsAverageQueryHandler : IRequestHandler<GetBookRatingsAverageQuery, double>
    {
        private readonly IRatingRepository _ratingRepository;

        public GetBookRatingsAverageQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<double> Handle(GetBookRatingsAverageQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _ratingRepository.GetRatingsByBookIdAsync(request.BookId);
            if (ratings.Count() == 0)
            {
                return 0;
            }
            return ratings.Average(r => r.Value);
        }

    }
}
