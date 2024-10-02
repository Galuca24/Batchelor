using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Queries.GetByBookId
{
    public class GetReviewByBookIdQuery : IRequest<GetReviewByBookIdQueryResponse>
    {
        public Guid BookId { get; set; }
    }
    
}
