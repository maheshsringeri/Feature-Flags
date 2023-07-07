using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Feature_Flags.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IFeatureManager featureManager, ILogger<WeatherForecastController> logger)
        {
            _featureManager = featureManager;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();

            var isRainEnabled = await _featureManager.IsEnabledAsync("RainEnabled");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                RainExpected = isRainEnabled ? $"{rng.Next(0, 100)}%" : null,
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Advanced")]
        [FeatureGate("AdvanceEnabled")]
        public async Task<IEnumerable<WeatherForecast>> GetAdvanced()
        {
            var useNewAlgorithm = await _featureManager.IsEnabledAsync("NewAlgorithmEnabled");
            return useNewAlgorithm ? await NewAlgorithm() : await OldAlgorithm();
        }

        private async Task<IEnumerable<WeatherForecast>> OldAlgorithm()
        {
            var rng = new Random();

            var isRainEnabled = await _featureManager.IsEnabledAsync("RainEnabled");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                RainExpected = isRainEnabled ? $"{rng.Next(0, 100)}% -Old" : null,
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [FeatureGate("Browser")]
        private async Task<IEnumerable<WeatherForecast>> NewAlgorithm()
        {
            var rng = new Random();

            var isRainEnabled = await _featureManager.IsEnabledAsync("RainEnabled");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                RainExpected = isRainEnabled ? $"{rng.Next(0, 100)}% -New" : null,
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}