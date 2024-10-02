using Licenta.Application.Features.Users;
using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Queries.GetByAudioBookId
{
    public class GetReviewByAudioBookIdQueryHandler : IRequestHandler<GetReviewByAudioBookIdQuery, GetReviewByAudioBookIdQueryResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserManager _userManager;
        private readonly IUserPhotoRepository _userPhotoRepository;

        public GetReviewByAudioBookIdQueryHandler(IReviewRepository reviewRepository, IUserManager userManager, IUserPhotoRepository userPhotoRepository)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<GetReviewByAudioBookIdQueryResponse> Handle(GetReviewByAudioBookIdQuery request, CancellationToken cancellationToken)
        {
            var reviewResult = await _reviewRepository.GetReviewsByAudioBookIdAsync(request.AudioBookId);

            if (!reviewResult.IsSuccess)
            {
                return new GetReviewByAudioBookIdQueryResponse
                {
                    Success = false,
                    Message = reviewResult.Error
                };
            }

            var reviews = reviewResult.Value;
            var reviewDtos = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                var createdBy = await _userManager.FindByIdAsync(review.UserId);

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
                        AudioBookId = review.AudioBookId.Value,
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

            return new GetReviewByAudioBookIdQueryResponse
            {
                Success = true,
                Reviews = reviewDtos
            };
        }
    }
}
