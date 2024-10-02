using Licenta.Application.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace Licenta.Application.Features.Ratings.Commands.GiveRatingToBook
{
    public class GiveRatingToBookCommandHandler : IRequestHandler<GiveRatingToBookCommand, GiveRatingToBookCommandResponse>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public GiveRatingToBookCommandHandler(IRatingRepository ratingRepository, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _ratingRepository = ratingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<GiveRatingToBookCommandResponse> Handle(GiveRatingToBookCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new GiveRatingToBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User not found" }
                };
            }


            var book = await _bookRepository.FindByIdAsync(request.BookId);
            if (book == null)
            {
                return new GiveRatingToBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Book not found" }
                };
            }

            var existingRating = await _ratingRepository.FindByBookAndUserIdAsync(request.BookId, request.UserId);

            if (existingRating != null)
            {
                existingRating.Value = request.Value;
                await _ratingRepository.UpdateAsync(existingRating);
            }
            else
            {
                var ratingResult = Domain.Entities.Rating.Create(request.Value, request.BookId, request.UserId);
                if (!ratingResult.IsSuccess)
                {
                    return new GiveRatingToBookCommandResponse
                    {
                        Success = false,
                        ValidationsErrors = new List<string> { ratingResult.Error }
                    };
                }
                await _ratingRepository.AddAsync(ratingResult.Value);
            }

            return new GiveRatingToBookCommandResponse
            {
                Success = true,
                Rating = new RatingDto
                {
                    BookId = request.BookId,
                    UserId = request.UserId,
                    Value = request.Value
                }
            };
        }
    }
}
