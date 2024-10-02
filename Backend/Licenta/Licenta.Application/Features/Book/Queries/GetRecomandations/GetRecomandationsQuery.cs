using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetRecomandations
{
    public class GetBookRecommendationsQuery : IRequest<List<BookDto>>
    {
        public Guid UserId { get; }

        public GetBookRecommendationsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
