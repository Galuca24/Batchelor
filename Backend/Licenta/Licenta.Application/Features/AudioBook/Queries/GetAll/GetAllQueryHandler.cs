using Licenta.Application.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Queries.GetAll
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<AudioBookDto>>
    {
        private readonly IAudioBookRepository _audioBookRepository;

        public GetAllQueryHandler(IAudioBookRepository audioBookRepository)
        {
            _audioBookRepository = audioBookRepository;
        }

        public async Task<List<AudioBookDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var audioBooksResult = await _audioBookRepository.GetAllWithChaptersAsync();
            var audioBookDtos = new List<AudioBookDto>();

            if (audioBooksResult.IsSuccess && audioBooksResult.Value != null)
            {
                foreach (var audioBook in audioBooksResult.Value)
                {
                    var chapterDtos = audioBook.Chapters.Select(chapter => new ChapterDto
                    {
                        ChapterId = chapter.ChapterId,
                        ChapterName = chapter.ChapterName,
                        AudioUrl = chapter.AudioUrl,
                        Duration = chapter.Duration
                    }).ToList();

                    audioBookDtos.Add(new AudioBookDto
                    {
                        AudioBookId = audioBook.AudioBookId,
                        Title = audioBook.Title,
                        Author = audioBook.Author,
                        Description = audioBook.Description,
                        ImageUrl = audioBook.ImageUrl,
                        AudioUrl = audioBook.AudioUrl,
                        Genre = audioBook.Genre,
                        Duration = audioBook.Duration,
                        ISBN = audioBook.ISBN,
                        Chapters = chapterDtos // Adaugă capitolele la DTO
                    });
                }
            }
            else
            {
                throw new Exception("Nu s-au putut obține cărțile audio din baza de date.");
            }

            return audioBookDtos;
        }
    }
}
