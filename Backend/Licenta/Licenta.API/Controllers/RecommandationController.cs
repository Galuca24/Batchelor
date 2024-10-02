using Licenta.Application.Contracts;
using Licenta.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Licenta.API.Controllers
{
    [ApiController]
    [Route("api/v1/recommendations")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public RecommendationsController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendations([FromQuery] string userRequest)
        {
            if (string.IsNullOrEmpty(userRequest))
            {
                return BadRequest("User request is required.");
            }

            try
            {
                var recommendations = await _openAIService.GetBookRecommendationsAsync(userRequest);
                return Ok(recommendations);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(429, ex.Message); // 429 Too Many Requests
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Internal Server Error
            }
        }
    }
}
