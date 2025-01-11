using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace WebScrapApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class WebScrapController(
        VnExpressScrapService vnExpressScrapService,
        TuoiTreScrapService tuoiTreScrapService
    ) : ControllerBase
    {
        private static readonly Counter requestTuoitreSuccessCounter = Metrics.CreateCounter(
            "tuoitre_requests_success",
            "Total number of successful requests (TuoiTre)."
        );
        private static readonly Counter requestTuoitreFailureCounter = Metrics.CreateCounter(
            "tuoitre_requests_failure",
            "Total number of failed requests (TuoiTre)."
        );
        private static readonly Counter requestTuoitreCounter = Metrics.CreateCounter(
            "tuoitre_requests_total",
            "Total number of requests (TuoiTre) received."
        );
        private static readonly Histogram requestTuoitreDuration = Metrics.CreateHistogram(
            "tuoitre_request_duration_seconds",
            "Request (TuoiTre) duration in seconds."
        );

        private static readonly Counter requestVnExpressSuccessCounter = Metrics.CreateCounter(
            "vnexpress_requests_success",
            "Total number of successful requests (VnExpress)."
        );
        private static readonly Counter requestVnExpressFailureCounter = Metrics.CreateCounter(
            "vnexpress_requests_failure",
            "Total number of failed requests (VnExpress)."
        );
        private static readonly Counter requestVnExpressCounter = Metrics.CreateCounter(
            "vnexpress_requests_total",
            "Total number of requests (VnExpress) received."
        );
        private static readonly Histogram requestVnExpressDuration = Metrics.CreateHistogram(
            "vnexpress_request_duration_seconds",
            "Request (VnExpress) duration in seconds."
        );
        private readonly VnExpressScrapService _vnExpressScrapService = vnExpressScrapService;
        private readonly TuoiTreScrapService _tuoiTreScrapService = tuoiTreScrapService;

        [HttpGet("vnexpress")]
        public async Task<IActionResult> GetVnExpress()
        {
            using (requestVnExpressDuration.NewTimer())
            {
                requestVnExpressCounter.Inc();
                try
                {
                    var data = await _vnExpressScrapService.GetContent();
                    requestVnExpressSuccessCounter.Inc();
                    return Ok(new { data });
                }
                catch (HttpRequestException e)
                {
                    requestVnExpressFailureCounter.Inc();
                    return StatusCode(500, $"Error fetching data: {e.Message}");
                }
                catch (JsonException e)
                {
                    requestVnExpressFailureCounter.Inc();
                    return StatusCode(500, $"Error parsing JSON: {e.Message}");
                }
            }
        }

        [HttpGet("tuoitre")]
        public async Task<IActionResult> GetTuoiTre()
        {
            using (requestTuoitreDuration.NewTimer())
            {
                requestTuoitreCounter.Inc();
                try
                {
                    requestTuoitreSuccessCounter.Inc();
                    var data = await _tuoiTreScrapService.GetContent();
                    return Ok(new { data });
                }
                catch (HttpRequestException e)
                {
                    requestTuoitreFailureCounter.Inc();
                    return StatusCode(500, $"Error fetching data: {e.Message}");
                }
            }
        }
    }
}
