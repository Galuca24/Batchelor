namespace Licenta.Application.Features.AudioBook
{
    public class ChapterDto
    {
        public Guid ChapterId { get; set; }
        public string ChapterName { get; set; }
        public string AudioUrl { get; set; }
        public string Duration { get; set; }
    }
}