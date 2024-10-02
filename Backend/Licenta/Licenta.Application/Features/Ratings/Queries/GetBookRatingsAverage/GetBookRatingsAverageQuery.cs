using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Ratings.Queries.GetBookRatingsAverage
{
    public class GetBookRatingsAverageQuery : IRequest<double>
    {
        public Guid BookId { get; set; }
        public double Rating { get; set; }
    }
}
