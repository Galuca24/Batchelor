using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, UpdateReviewCommandResponse>
    {
        private readonly IReviewRepository _reviewRepository;

        public UpdateReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<UpdateReviewCommandResponse> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var result = await _reviewRepository.FindByIdAsync(request.ReviewId);
            if (!result.IsSuccess || result.Value == null) // Ensure result is successful and contains a value
            {
                return new UpdateReviewCommandResponse { Success = false, Message = "Review not found." };
            }

            var review = result.Value;
            review.ReviewText = request.ReviewText;
            review.DatePosted = DateTime.UtcNow; // Updating the date

            var updateResult = await _reviewRepository.UpdateAsync(review);
            if (!updateResult.IsSuccess)
            {
                return new UpdateReviewCommandResponse { Success = false, Message = "Eroare la update!" }; // Assuming there's an ErrorMessage field
            }

            return new UpdateReviewCommandResponse { Success = true, Message = "Review updated successfully." };
        }
    }

}
