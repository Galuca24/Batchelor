using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, CreateReviewCommandResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserManager _userRepository;

        public CreateReviewCommandHandler(IReviewRepository reviewRepository, IBookRepository bookRepository, IUserManager userRepository)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<CreateReviewCommandResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateReviewCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new CreateReviewCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var userExists = await _userRepository.UserExists(request.UserId);
            if (userExists == null)
            {
                return new CreateReviewCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Utilizatorul nu exista." }
                };
            }

            var bookExists = await _bookRepository.BookExists(request.BookId);

            if (!bookExists)
            {
                return new CreateReviewCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Cartea nu exista." }
                };
            }

            var review = Review.Create(request.UserId, request.BookId, null, request.ReviewText, request.CreatedAt);

            if (!review.IsSuccess)
            {
                return new CreateReviewCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { review.Error }
                };
            }

            await _reviewRepository.AddAsync(review.Value);
            return new CreateReviewCommandResponse
            {
                Success = true,
                Review = new CreateReviewDto
                {
                    UserId = review.Value.UserId,
                    BookId = review.Value.BookId.Value,
                    ReviewText = review.Value.ReviewText,
                    CreatedAt = review.Value.DatePosted
                }
            };
        }


    }
}
