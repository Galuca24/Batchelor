using Licenta.API.Controllers;
using Licenta.API.Models;

namespace Licenta.API.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsItem>> GetLatestNewsAsync();
    }
}
