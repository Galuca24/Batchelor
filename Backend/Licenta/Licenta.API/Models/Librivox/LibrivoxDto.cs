using Licenta.Domain.Entities;
using static LibrivoxService;

namespace Licenta.API.Models.Librivox
{
    public class LibrivoxDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string UrlLibrivox { get; set; }
        public string UrlAudio { get; set; }
        public TimeSpan Duration { get; set; }
        public string Genre { get; set; }
        public List<Chapter> Chapters { get; set; } 
       // public List<AudioChapter> Chapters { get; set; }

    }



}
