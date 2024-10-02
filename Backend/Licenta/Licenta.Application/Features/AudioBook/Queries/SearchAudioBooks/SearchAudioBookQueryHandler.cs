using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Queries.SearchAudioBooks
{
    public class SearchAudioBookQueryHandler : IRequestHandler<SearchAudioBookQuery, SearchAudioBookQueryResponse>
    {
        private readonly IAudioBookRepository audioBookRepository;
        private readonly IChapterRepository chapterRepository; // Adăugăm repository-ul pentru capitole

        public SearchAudioBookQueryHandler(IAudioBookRepository audioBookRepository, IChapterRepository chapterRepository)
        {
            this.audioBookRepository = audioBookRepository;
            this.chapterRepository = chapterRepository;
        }

        public async Task<SearchAudioBookQueryResponse> Handle(SearchAudioBookQuery request, CancellationToken cancellationToken)
        {
            var allAudioBooks = await audioBookRepository.GetAllAsync();
            if (!allAudioBooks.IsSuccess)
            {
                return new SearchAudioBookQueryResponse { Success = false, Message = allAudioBooks.Error };
            }

            var audioBooks = allAudioBooks.Value.Where(b =>
                           (!string.IsNullOrWhiteSpace(b.Title) && b.Title.ToLower().Contains(request.SearchValue.ToLower())) ||
                                          (!string.IsNullOrWhiteSpace(b.Author) && b.Author.ToLower().Contains(request.SearchValue.ToLower())) ||
                                                         (!string.IsNullOrWhiteSpace(b.Genre) && b.Genre.ToLower().Contains(request.SearchValue.ToLower()))
                                                                                                 ).ToArray();

            var audioBookDtos = new List<SearchAudioBookDto>();
            foreach (var b in audioBooks)
            {
                // Obține capitolele pentru fiecare audiobook
                var chaptersResult = await chapterRepository.GetChaptersByAudioBookIdAsync(b.AudioBookId);
                var chapters = chaptersResult.IsSuccess
                               ? chaptersResult.Value.Select(c => new ChapterDto
                               {
                                   ChapterId = c.ChapterId,
                                   ChapterName = c.ChapterName,
                                   AudioUrl = c.AudioUrl,
                                   Duration = c.Duration
                               }).ToList()
                               : new List<ChapterDto>();

                audioBookDtos.Add(new SearchAudioBookDto
                {
                    AudioBookId = b.AudioBookId,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                    Genre = b.Genre,
                    ISBN = b.ISBN,
                    Chapters = chapters
                });
            }

            return new SearchAudioBookQueryResponse
            {
                Success = true,
                AudioBooks = audioBookDtos.Take(25).ToArray()
            };
        }
    }
}
