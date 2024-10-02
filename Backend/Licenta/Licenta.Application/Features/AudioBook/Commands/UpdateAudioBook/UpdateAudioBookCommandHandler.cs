using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Commands.UpdateAudioBook
{
    public class UpdateAudioBookCommandHandler : IRequestHandler<UpdateAudioBookCommand, AudioBookDto>
    {
        private readonly IAudioBookRepository _audioBookRepository;

        public UpdateAudioBookCommandHandler(IAudioBookRepository audioBookRepository)
        {
            _audioBookRepository = audioBookRepository;
        }

        public async Task<AudioBookDto> Handle(UpdateAudioBookCommand request, CancellationToken cancellationToken)
        {
            var result = await _audioBookRepository.FindByIdAsync(request.AudioBookId);

            if (!result.IsSuccess || result.Value == null)
            {
                throw new KeyNotFoundException("AudioBook not found.");
            }

            var audioBook = result.Value;

            // Asigură-te că nu actualizezi cu valori null dacă DTO-ul nu le include
            audioBook.Title = request.AudioBook.Title ?? audioBook.Title;
            audioBook.Author = request.AudioBook.Author ?? audioBook.Author;
            audioBook.ISBN = request.AudioBook.ISBN ?? audioBook.ISBN;
            audioBook.Genre = request.AudioBook.Genre ?? audioBook.Genre;
            audioBook.Description = request.AudioBook.Description ?? audioBook.Description;
            audioBook.ImageUrl = request.AudioBook.ImageUrl ?? audioBook.ImageUrl;
            audioBook.AudioUrl = request.AudioBook.AudioUrl ?? audioBook.AudioUrl;
            audioBook.Duration = request.AudioBook.Duration ?? audioBook.Duration;

            await _audioBookRepository.UpdateAsync(audioBook);

            return new AudioBookDto
            {
                AudioBookId = audioBook.AudioBookId,
                Title = audioBook.Title,
                Author = audioBook.Author,
                ISBN = audioBook.ISBN,
                Genre = audioBook.Genre,
                Description = audioBook.Description,
                ImageUrl = audioBook.ImageUrl,
                AudioUrl = audioBook.AudioUrl,
                Duration = audioBook.Duration
            };
        }
    }
    
}
