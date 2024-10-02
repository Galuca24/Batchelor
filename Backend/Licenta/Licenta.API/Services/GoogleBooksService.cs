using Licenta.API.Models;
using Newtonsoft.Json;

namespace Licenta.API.Services
{

   
    public class GoogleBooksService : IGoogleBooksService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "";
        }

        public async Task<List<GoogleBooksDto>> SearchGoogleBooks(string query)
        {
            var url = $"https://www.googleapis.com/books/v1/volumes?q={query}&key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<GoogleBooksApiResponse>(content);

            return books.Items.Select(item => new GoogleBooksDto
            {
                Title = item.VolumeInfo.Title,
                Author = item.VolumeInfo.Authors != null ? string.Join(", ", item.VolumeInfo.Authors) : "Unknown Author",
                Description = item.VolumeInfo.Description,
                ImageUrl = item.VolumeInfo.ImageLinks?.Thumbnail!,
                GoogleBooksId = item.Id,
                Genre = item.VolumeInfo.Categories != null ? string.Join(", ", item.VolumeInfo.Categories) : "No Genre Specified"
            }).ToList();
        }

    }

    public class GoogleBooksApiResponse
    {
        public IEnumerable<GoogleBook> Items { get; set; }
    }

    public class GoogleBook
    {
        public string Id { get; set; }
        public VolumeInfo VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string Title { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public string Description { get; set; }
        public ImageLinks ImageLinks { get; set; }

        public IEnumerable<string> Categories { get; set; }  // Adaugă această linie
    }

    public class ImageLinks
    {
        public string Thumbnail { get; set; }
    }

}
