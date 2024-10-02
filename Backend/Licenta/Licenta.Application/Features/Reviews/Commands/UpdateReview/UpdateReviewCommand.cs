using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest<UpdateReviewCommandResponse>
    {
        public Guid ReviewId { get; set; }
        public string ReviewText { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
