using MediatR;
using System;

namespace Licenta.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<CreateReviewCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string ReviewText { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
