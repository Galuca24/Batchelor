using Licenta.API.Models.Librivox;

namespace Licenta.API.Services
{
    public interface ILibrivoxService
    {
        Task<List<LibrivoxDto>> SearchLibrivoxBooksAsync(string query);
    }

}
