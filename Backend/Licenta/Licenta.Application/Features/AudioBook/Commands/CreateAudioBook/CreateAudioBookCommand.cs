using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Commands.CreateAudioBook
{
    public class CreateAudioBookCommand : IRequest<CreateAudioBookCommandResponse>
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string AudioUrl { get; set; }
        public string Duration { get; set; }  // Modificat pentru a primi ca string
        public string ImageUrl { get; set; }
        public List<ChapterDto> Chapters { get; set; } = new List<ChapterDto>();
    }
}
