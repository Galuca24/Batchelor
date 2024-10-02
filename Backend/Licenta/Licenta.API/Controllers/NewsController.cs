using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Licenta.API.Models;
using Licenta.API.Services;

namespace Licenta.API.Controllers
{
    [ApiController]
    [Route("api/v1/news")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestNews()
        {
            try
            {
                var news = await _newsService.GetLatestNewsAsync();
                return Ok(news);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
