using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers;

[ApiController]
[Route("weather")]
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
    
    private readonly SettingsClient _settingsClient;

    public WeatherForecastController(
        SettingsClient settingsClient
    )
    {
        _settingsClient = settingsClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var getSetting = bool.TryParse(await _settingsClient.GetSetting(EnableWeatherForecast), out var enable);
        
        if (!getSetting || !enable)
        {
            throw new BadHttpRequestException("This feature is disabled.", 400);
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
