using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Commands.CreateAudioBook
{
    public class CreateAudioBookCommandHandler : IRequestHandler<CreateAudioBookCommand, CreateAudioBookCommandResponse>
    {
        private readonly IAudioBookRepository _audioBookRepository;
        private readonly IChapterRepository _chapterRepository;

        public CreateAudioBookCommandHandler(IAudioBookRepository audioBookRepository, IChapterRepository chapterRepository)
        {
            _audioBookRepository = audioBookRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<CreateAudioBookCommandResponse> Handle(CreateAudioBookCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateAudioBookCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new CreateAudioBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            // Conversia duratei din string în TimeSpan
            if (!TimeSpan.TryParse(request.Duration, out var duration))
            {
                return new CreateAudioBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Invalid duration format. Please use hh:mm:ss format." }
                };
            }

            var audioBook = Domain.Entities.AudioBook.Create(
                request.Title,
                request.Author,
                request.Genre,
                request.Description,
                request.ISBN,
                request.AudioUrl,
                duration,
                request.ImageUrl);

            if (!audioBook.IsSuccess)
            {
                return new CreateAudioBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { audioBook.Error }
                };
            }

            // Adaugă cartea audio
            await _audioBookRepository.AddAsync(audioBook.Value);

            // Adaugă capitolele asociate
            foreach (var chapterDto in request.Chapters)
            {
                var chapter = new Domain.Entities.Chapter(Guid.NewGuid(), chapterDto.ChapterName, chapterDto.AudioUrl, chapterDto.Duration, audioBook.Value.AudioBookId);
                await _chapterRepository.AddAsync(chapter);
            }

            return new CreateAudioBookCommandResponse
            {
                Success = true,
                AudioBook = new CreateAudioBookDto
                {
                    AudioBookId = audioBook.Value.AudioBookId,
                    Title = audioBook.Value.Title,
                    Author = audioBook.Value.Author,
                    Genre = audioBook.Value.Genre,
                    Description = audioBook.Value.Description,
                    ISBN = audioBook.Value.ISBN,
                    AudioUrl = audioBook.Value.AudioUrl,
                    Duration = audioBook.Value.Duration,
                    ImageUrl = audioBook.Value.ImageUrl,
                    Chapters = request.Chapters // Returnează capitolele adăugate
                }
            };
        }
    }
}
