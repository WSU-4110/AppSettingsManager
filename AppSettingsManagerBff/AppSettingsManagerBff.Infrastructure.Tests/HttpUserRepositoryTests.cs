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
    
    [Theory]
    [AutoTestData]
    public async Task CreateUser_Succeeds(CreateUserRequest request)
    {
        var user = new User
        {
            UserId = request.UserId,
            Password = request.Password,
            Email = request.Email
        };
        
        var expectedResponse = GetHttpResponseMessage(HttpStatusCode.OK, user);
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(expectedResponse);
        
        var repository = GetHttpUserRepository();
        
        var result = await repository.CreateUser(request);
        
        Assert.Equal(user.UserId, result.UserId);
    }
    
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