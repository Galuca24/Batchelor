using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.CreateReviewForAudioBook
{
    public class CreateReviewForAudioBookCommandHandler : IRequestHandler<CreateReviewForAudioBookCommand, CreateReviewForAudioBookCommandResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IAudioBookRepository _audioBookRepository;
        private readonly IUserManager _userRepository;

        public CreateReviewForAudioBookCommandHandler(IReviewRepository reviewRepository, IAudioBookRepository audioBookRepository, IUserManager userRepository)
        {
            _reviewRepository = reviewRepository;
            _audioBookRepository = audioBookRepository;
            _userRepository = userRepository;
        }

        public async Task<CreateReviewForAudioBookCommandResponse> Handle(CreateReviewForAudioBookCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateReviewForAudioBookCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new CreateReviewForAudioBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var userExists = await _userRepository.UserExists(request.UserId);
            if (userExists == null)
            {
                return new CreateReviewForAudioBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Utilizatorul nu exista." }
                };
            }

            var audioBookExists = await _audioBookRepository.AudioBookExists(request.AudioBookId);

            if (!audioBookExists)
            {
                return new CreateReviewForAudioBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Cartea audio nu exista." }
                };
            }

            var review = Review.Create(request.UserId, null, request.AudioBookId, request.ReviewText, request.CreatedAt);

            if (!review.IsSuccess)
            {
                return new CreateReviewForAudioBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Recenzia nu a putut fi creata." }
                };
            }
            await _reviewRepository.AddAsync(review.Value);
            return new CreateReviewForAudioBookCommandResponse
            {
                Success = true,
                Review = new CreateReviewForAudioDto
                {
                    UserId = review.Value.UserId,
                    AudioBookId = review.Value.AudioBookId.Value,
                    ReviewText = review.Value.ReviewText,
                    CreatedAt = review.Value.DatePosted
                }
            };
        }

    }
}
