using Licenta.API.Models;

namespace Licenta.API.Services
{
    public interface IGoogleBooksService
    {
        Task<List<GoogleBooksDto>> SearchGoogleBooks(string query);
    }
}
