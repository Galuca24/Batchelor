using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Licenta.Application.Contracts;
using Licenta.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using Licenta.Application.Contracts.Interfaces;
using Licenta.Application.Persistence;
using System.Text.Json.Serialization;

namespace Infrastructure.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IBookRepository bookRepository, ILogger<OpenAIService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<string> GetBookRecommendationsAsync(string userRequest)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OpenAI API key is not configured.");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var books = await _bookRepository.GetAvailableBooksAsync();
            var bookList = string.Join("\n", books.Select(b => $"Title: {b.Title}, Author: {b.Author}, Genre: {b.Genre} , Description: {b.Description}"));

            var prompt = $"Here are some books available in our library:\n{bookList}\n\nBased on the user's request: '{userRequest}', recommend some books.";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "You are a helpful assistant." },
            new { role = "user", content = prompt }
        },
                max_tokens = 300,  
                temperature = 0.2
            };

            _logger.LogInformation($"Sending request to OpenAI: {JsonSerializer.Serialize(requestBody)}");

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions",
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Received response from OpenAI: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"OpenAI request failed. Status code: {response.StatusCode}, Response: {responseContent}");
                throw new HttpRequestException($"Failed to fetch recommendations from OpenAI. Status code: {response.StatusCode}, Response: {responseContent}");
            }

            var result = JsonSerializer.Deserialize<ChatResponse>(responseContent);

            if (result?.Choices == null || !result.Choices.Any())
            {
                throw new InvalidOperationException("The response from OpenAI did not contain any choices.");
            }

            return result.Choices.FirstOrDefault()?.Message.Content;
        }

    }


    public class ChatResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

   

}
