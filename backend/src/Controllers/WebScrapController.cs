using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WebScrapApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class WebScrapController(
        VnExpressScrapService vnExpressScrapService,
        TuoiTreScrapService tuoiTreScrapService
    ) : ControllerBase
    {
        private readonly VnExpressScrapService _vnExpressScrapService = vnExpressScrapService;
        private readonly TuoiTreScrapService _tuoiTreScrapService = tuoiTreScrapService;

        [HttpGet("vnexpress")]
        public async Task<IActionResult> GetVnExpress()
        {
            try
            {
                var data = await _vnExpressScrapService.GetContent();
                return Ok(new { data });
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, $"Error fetching data: {e.Message}");
            }
            catch (JsonException e)
            {
                return StatusCode(500, $"Error parsing JSON: {e.Message}");
            }
        }

        [HttpGet("tuoitre")]
        public async Task<IActionResult> GetTuoiTre()
        {
            try
            {
                var data = await _tuoiTreScrapService.GetContent();
                return Ok(new { data });
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, $"Error fetching data: {e.Message}");
            }
        }
    }
}
