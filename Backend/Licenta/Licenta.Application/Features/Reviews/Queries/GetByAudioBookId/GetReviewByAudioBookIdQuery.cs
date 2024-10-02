using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Queries.GetByAudioBookId
{
    public class GetReviewByAudioBookIdQuery : IRequest<GetReviewByAudioBookIdQueryResponse>
    {
        public Guid AudioBookId { get; set; }
    }
}
