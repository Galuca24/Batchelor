namespace Licenta.API.Models.Librivox
{
    public class LibrivoxBook
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string UrlLibrivox { get; set; }
        public string Totaltime { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string UrlImage { get; set; }
        public string[] Genres { get; set; }
    }
}
