using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook
{
    public class AudioBookDto
    {
        public Guid AudioBookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public TimeSpan Duration { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string AudioUrl { get; set; }
        public string Genre { get; set; }
        public List<ChapterDto> Chapters { get; set; } 

    }
}
