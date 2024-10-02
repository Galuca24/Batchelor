using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Queries.GetByAudioBookId
{
    public class GetReviewByAudioBookIdQueryResponse : BaseResponse
    {
        public List<ReviewDto> Reviews { get; set; } // Ensure this is public
    }
}
