using AppSettingsManagerBff.Infrastructure.ApiRepositories;
using System.Net;
using System.Text.Json;
using AppSettingsManagerBff.Model;
using Moq.Protected;

namespace AppSettingsManagerBff.Infrastructure.Tests;

public class HttpSettingsRepositoryTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();

    private readonly AppSettingsManagerApiConfig _config = new AppSettingsManagerApiConfig
    {
        BaseAddress = "http://localhost:5000/"
    };

    private HttpSettingsRepository GetHttpSettingsRepository()
    {
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_config.BaseAddress + "settings/")
        };

        return new HttpSettingsRepository(httpClient, _config);
    }
    
    private HttpResponseMessage GetHttpResponseMessage(HttpStatusCode statusCode, object content)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(content))
        };
    }
}