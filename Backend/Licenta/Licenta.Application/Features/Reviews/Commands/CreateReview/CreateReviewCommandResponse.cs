using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandResponse : BaseResponse
    {
        public CreateReviewCommandResponse() : base()
        {
        }

        public CreateReviewDto Review { get; set; }
    }
}
