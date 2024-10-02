using Licenta.Application.Features.Users;
using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Queries.GetByBookId
{
    public class GetReviewByBookIdQueryHandler : IRequestHandler<GetReviewByBookIdQuery, GetReviewByBookIdQueryResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserManager _userManager;  // Assuming this is the interface for user management
        private readonly IUserPhotoRepository _userPhotoRepository;  // Repository to get user photos

        public GetReviewByBookIdQueryHandler(IReviewRepository reviewRepository, IUserManager userManager, IUserPhotoRepository userPhotoRepository)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<GetReviewByBookIdQueryResponse> Handle(GetReviewByBookIdQuery request, CancellationToken cancellationToken)
        {
            var reviewResult = await _reviewRepository.GetReviewsByBookIdAsync(request.BookId);

            if (!reviewResult.IsSuccess)
            {
                return new GetReviewByBookIdQueryResponse
                {
                    Success = false,
                    Message = reviewResult.Error
                };
            }

            var reviews = reviewResult.Value;
            var reviewDtos = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                var createdBy = await _userManager.FindByIdAsync(review.UserId);  // Assuming FindByIdAsync is available

                if (createdBy.IsSuccess)
                {
                    var userPhoto = await _userPhotoRepository.GetUserPhotoByUserIdAsync(createdBy.Value.UserId);
                    createdBy.Value.UserPhoto = userPhoto.IsSuccess ? new UserCloudPhotoDto
                    {
                        UserPhotoId = userPhoto.Value.UserPhotoId,
                        PhotoUrl = userPhoto.Value.PhotoUrl
                    } : null;

                    reviewDtos.Add(new ReviewDto
                    {
                        ReviewId = review.ReviewId,
                        BookId = review.BookId.Value,
                        ReviewText = review.ReviewText,
                        DatePosted = review.DatePosted,
                        CreatedBy = new UserReviewDto
                        {
                            UserId = createdBy.Value.UserId,
                            Username = createdBy.Value.Username,
                            Name = createdBy.Value.Name,
                            Email = createdBy.Value.Email,
                            UserPhoto = createdBy.Value.UserPhoto
                        }
                    });
                }
            }

            return new GetReviewByBookIdQueryResponse
            {
                Success = true,
                Reviews = reviewDtos
            };
        }

    }

}
