using System;
using System.Collections.Generic;

namespace Licenta.API.Models
{
    public class NewsApiResponse
    {
        public IEnumerable<NewsItem> Articles { get; set; }
    }

    public class NewsItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
    }
}
