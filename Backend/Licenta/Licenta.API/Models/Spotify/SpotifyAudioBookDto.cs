namespace Licenta.API.Models.Spotify
{
    public class SpotifyAudioBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string UrlSpotify { get; set; }
        public TimeSpan Duration { get; set; }
        public string Genre { get; set; }
    }

}
