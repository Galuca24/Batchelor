using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteCommentCommand, DeleteReviewCommandResponse>
    {
        private readonly IReviewRepository _reviewRepository;

        public DeleteReviewCommandHandler(IReviewRepository commentRepository)
        {
            _reviewRepository = commentRepository;
        }

        public async Task<DeleteReviewCommandResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var result = await _reviewRepository.FindByIdAsync(request.ReviewId);
            if (!result.IsSuccess)
            {
                return new DeleteReviewCommandResponse { Success = false, Message = "Review not found." };
            }

            await _reviewRepository.DeleteAsync(result.Value.ReviewId); // Ensure this method accepts a Guid
            return new DeleteReviewCommandResponse { Success = true, Message = "Review deleted successfully." };
        }

    }
}
