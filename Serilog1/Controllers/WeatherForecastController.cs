using Microsoft.AspNetCore.Mvc;
using Serilog1.Interceptors;
using System.Diagnostics;

namespace Serilog1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger = logger;

        [CustomInterceptor1]
        [HttpGet(Name = "GetWeatherForecast")]
        public void Get()
        {
            var traceId = HttpContext.TraceIdentifier ?? Request.Headers["X-TraceId"].FirstOrDefault();
            _logger.LogInformation("Info Logu gönderiyorum! Mevcut traceId: {@TraceId}", traceId);
        }

        [CustomInterceptor1]
        [HttpGet("[action]")]
        public IActionResult ErrorTest1()
        {
            var traceId = HttpContext.TraceIdentifier ?? Request.Headers["X-TraceId"].FirstOrDefault();
            _logger.LogError("Info Logu gönderiyorum! Mevcut traceId: {@TraceId}", traceId);
            Response.StatusCode = 399;
            return BadRequest();
        }
    }
}
