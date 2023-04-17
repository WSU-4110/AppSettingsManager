using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };
    
    private const string EnableWeatherForecast = "EnableWeatherForecast";

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly SettingsClient _settingsClient;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        SettingsClient settingsClient
    )
    {
        _logger = logger;
        _settingsClient = settingsClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var getSetting = bool.TryParse(await _settingsClient.GetSetting(EnableWeatherForecast), out var enable);
        
        if (!getSetting || !enable)
        {
            throw new Exception("This feature is disabled");
        }
        
        return Enumerable
            .Range(1, 5)
            .Select(
                index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    }
            )
            .ToArray();
    }
}
