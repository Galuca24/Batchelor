using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Licenta.API.Models.Librivox;
using Licenta.API.Services;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using Licenta.Domain.Entities;

public class LibrivoxService : ILibrivoxService
{
    private readonly HttpClient _httpClient;

    public LibrivoxService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

  
    public async Task<List<LibrivoxDto>> SearchLibrivoxBooksAsync(string query)
    {
        var url = $"https://librivox.org/api/feed/audiobooks?title={query}&format=json";
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
            throw new Exception("Failed to fetch data from Librivox API.", ex);
        }

        var content = await response.Content.ReadAsStringAsync();
        var jsonResponse = JObject.Parse(content);
        var books = jsonResponse["books"] as JArray;

        if (books == null || !books.Any())
        {
            throw new Exception("No results found.");
        }

        var result = new List<LibrivoxDto>();
        foreach (var item in books)
        {
            var rssUrl = (string)item["url_rss"];
            var audioUrls = await GetAudioUrlsFromRssAsync(rssUrl);

            var librivoxUrl = (string)item["url_librivox"];
            var imageUrl = await GetImageUrlFromLibrivoxPageAsync(librivoxUrl);
            var genre = await GetGenreFromLibrivoxPageAsync(librivoxUrl);
            var chapters = await GetChaptersFromLibrivoxPageAsync(librivoxUrl);

            result.Add(new LibrivoxDto
            {
                Title = (string)item["title"],
                Author = item["authors"] != null ? string.Join(", ", item["authors"].Select(a => $"{a["first_name"]} {a["last_name"]}")) : "Unknown Author",
                Description = (string)item["description"],
                ImageUrl = imageUrl,
                UrlLibrivox = librivoxUrl,
                UrlAudio = audioUrls.FirstOrDefault(), // Păstrează primul audio URL pentru compatibilitate
                Duration = TimeSpan.TryParse((string)item["totaltime"], out var duration) ? duration : TimeSpan.Zero,
                Genre = genre,
                Chapters = chapters // Adaugă capitolele
            });
        }

        return result;
    }


    private async Task<List<Chapter>> GetChaptersFromLibrivoxPageAsync(string librivoxUrl)
    {
        if (string.IsNullOrEmpty(librivoxUrl))
            return new List<Chapter>();

        var response = await _httpClient.GetAsync(librivoxUrl);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content);

        var chapters = new List<Chapter>();

        var chapterNodes = htmlDoc.DocumentNode.SelectNodes("//tbody/tr");
        if (chapterNodes != null)
        {
            foreach (var chapterNode in chapterNodes)
            {
                var chapterNameNode = chapterNode.SelectSingleNode(".//td[2]/a");
                var audioUrlNode = chapterNode.SelectSingleNode(".//td[1]/a");
                var durationNode = chapterNode.SelectSingleNode(".//td[4]");

                if (chapterNameNode != null && audioUrlNode != null && durationNode != null)
                {
                    var chapter = new Chapter(
                        Guid.NewGuid(),
                        chapterNameNode.InnerText.Trim(),
                        audioUrlNode.GetAttributeValue("href", ""),
                        durationNode.InnerText.Trim(),
                        Guid.Empty // Placeholder pentru AudioBookId
                    );
                    chapters.Add(chapter);
                }
            }
        }

        return chapters;
    }


    private async Task<string> GetAudioUrlFromRssAsync(string rssUrl)
    {
        if (string.IsNullOrEmpty(rssUrl))
            return "";

        var response = await _httpClient.GetAsync(rssUrl);
        response.EnsureSuccessStatusCode();

        using (var stream = await response.Content.ReadAsStreamAsync())
        using (var reader = XmlReader.Create(stream))
        {
            var feed = SyndicationFeed.Load(reader);
            var firstItem = feed.Items.FirstOrDefault();
            if (firstItem != null)
            {
                var enclosure = firstItem.Links.FirstOrDefault(link => link.RelationshipType == "enclosure");
                if (enclosure != null)
                {
                    return enclosure.Uri.ToString();
                }
            }
        }

        return "";
    }

    private async Task<string> GetImageUrlFromLibrivoxPageAsync(string librivoxUrl)
    {
        if (string.IsNullOrEmpty(librivoxUrl))
            return "";

        var response = await _httpClient.GetAsync(librivoxUrl);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content);

        var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='book-page-book-cover']//img");
        if (imageNode != null)
        {
            return imageNode.GetAttributeValue("src", "");
        }

        return "";
    }

    private async Task<string> GetGenreFromLibrivoxPageAsync(string librivoxUrl)
    {
        if (string.IsNullOrEmpty(librivoxUrl))
            return "No Genre Specified";

        var response = await _httpClient.GetAsync(librivoxUrl);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content);

        var genreNode = htmlDoc.DocumentNode.SelectSingleNode("//p[@class='book-page-genre']//span/following-sibling::text()");
        if (genreNode != null)
        {
            return genreNode.InnerText.Trim();
        }

        return "No Genre Specified";
    }

   


    private async Task<List<string>> GetAudioUrlsFromRssAsync(string rssUrl)
    {
        var audioUrls = new List<string>();

        if (string.IsNullOrEmpty(rssUrl))
            return audioUrls;

        var response = await _httpClient.GetAsync(rssUrl);
        response.EnsureSuccessStatusCode();

        using (var stream = await response.Content.ReadAsStreamAsync())
        using (var reader = XmlReader.Create(stream))
        {
            var feed = SyndicationFeed.Load(reader);
            foreach (var item in feed.Items)
            {
                var enclosure = item.Links.FirstOrDefault(link => link.RelationshipType == "enclosure");
                if (enclosure != null)
                {
                    audioUrls.Add(enclosure.Uri.ToString());
                }
            }
        }

        return audioUrls;
    }


    public class AudioChapter
    {
        public string AudioUrl { get; set; }
        public string ChapterName { get; set; }
        public string Duration { get; set; }
    }



}
