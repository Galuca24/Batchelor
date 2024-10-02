using Licenta.API.Models.Spotify;

namespace Licenta.API.Services
{
    public interface ISpotifyService
    {
        Task<List<SpotifyAudioBookDto>> SearchSpotifyAudioBooksAsync(string query);
    }
}
