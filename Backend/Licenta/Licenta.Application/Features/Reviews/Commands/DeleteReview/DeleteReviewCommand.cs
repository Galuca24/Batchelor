using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteCommentCommand : IRequest<DeleteReviewCommandResponse>
    {
        public Guid ReviewId { get; set; }


    }
}
