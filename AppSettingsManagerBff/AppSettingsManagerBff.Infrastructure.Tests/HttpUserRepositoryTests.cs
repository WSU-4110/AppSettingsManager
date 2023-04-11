using System.Net;
using System.Text.Json;
using AppSettingsManagerBff.Infrastructure.ApiRepositories;
using AppSettingsManagerBff.Model;
using AppSettingsManagerBff.Model.Requests;
using Moq.Protected;

namespace AppSettingsManagerBff.Infrastructure.Tests;

public class HttpUserRepositoryTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();
    
    private readonly AppSettingsManagerApiConfig _config = new AppSettingsManagerApiConfig
    {
        BaseAddress = "http://localhost:5000/"
    };

    private HttpUserRepository GetHttpUserRepository()
    {
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_config.BaseAddress + "users/")
        };
        
        return new HttpUserRepository(httpClient, _config);
    }
    
    private HttpResponseMessage GetHttpResponseMessage(HttpStatusCode statusCode, object content)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(content))
        };
    }
}