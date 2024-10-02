using Licenta.API.Controllers;
using Licenta.API.Models;

namespace Licenta.API.Services
{
    public class NewsService : INewsService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<NewsItem>> GetLatestNewsAsync()
        {
            var apiKey = _configuration["NewsApiKey"];
            var client = _httpClientFactory.CreateClient();

            // Setează header-ul User-Agent
            client.DefaultRequestHeaders.UserAgent.ParseAdd("SmartLibrary/1.0");

            var response = await client.GetAsync($"https://newsapi.org/v2/everything?q=books AND authors OR libraries AND reading OR book reviews&apiKey={apiKey}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch news: {response.ReasonPhrase}");
            }

            var news = await response.Content.ReadFromJsonAsync<NewsApiResponse>();

            var filteredNews = news.Articles
                .Where(article => !string.IsNullOrEmpty(article.UrlToImage))
                .Where(article => IsRelevantArticle(article))
                .ToList();

            return filteredNews;
        }

        private bool IsRelevantArticle(NewsItem article)
        {
            var keywords = new[] { "book", "author", "library", "reading", "review" };
            return keywords.Any(keyword => article.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                           article.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
