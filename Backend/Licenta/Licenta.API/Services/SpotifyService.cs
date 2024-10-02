using Licenta.API.Models.Spotify;
using SpotifyAPI.Web;

namespace Licenta.API.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly SpotifyClient _spotifyClient;

        public SpotifyService(string clientId, string clientSecret)
        {
            var config = SpotifyClientConfig.CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, clientSecret));

            _spotifyClient = new SpotifyClient(config);
        }

        public async Task<List<SpotifyAudioBookDto>> SearchSpotifyAudioBooksAsync(string query)
        {
            var searchRequest = new SearchRequest(SearchRequest.Types.Track, query);
            var searchResponse = await _spotifyClient.Search.Item(searchRequest);

            var audioBooks = searchResponse.Tracks.Items
                .Where(track => track.Name.ToLower().Contains("audiobook") || track.Album.Name.ToLower().Contains("audiobook") ||
                                track.Name.ToLower().Contains(query.ToLower()) || track.Album.Name.ToLower().Contains(query.ToLower()))
                .Select(track => new SpotifyAudioBookDto
                {
                    Title = track.Name,
                    Author = track.Artists.Select(artist => artist.Name).FirstOrDefault(),
                    Description = track.Album.Name,
                    ImageUrl = track.Album.Images.FirstOrDefault()?.Url,
                    UrlSpotify = track.ExternalUrls["spotify"],
                    Duration = TimeSpan.FromMilliseconds(track.DurationMs),
                    Genre = "No Genre Specified"
                }).ToList();

            return audioBooks;
        }
    }

}
