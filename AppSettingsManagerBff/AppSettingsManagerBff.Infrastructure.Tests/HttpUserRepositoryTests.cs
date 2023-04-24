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

    private readonly HttpClient _httpClient;

    public HttpUserRepositoryTests()
    {
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

        _httpClient.BaseAddress = new Uri(_config.BaseAddress + "users/");
    }

    [Theory]
    [AutoTestData]
    public async Task AuthenticateUser_ReturnsTrue_WhenUserIsAuthenticated(
        string userId,
        string password
    )
    {
        SetupHttpMessageHandler(HttpStatusCode.OK, true);

        var httpUserRepository = GetHttpUserRepository();

        var result = await httpUserRepository.AuthenticateUser(userId, password);

        Assert.True(result);
    }

    [Theory]
    [AutoTestData]
    public async Task AuthenticateUser_ReturnsFalse_WhenUserIsNotAuthenticated(
        string userId,
        string password
    )
    {
        SetupHttpMessageHandler(HttpStatusCode.NotFound, false);

        var httpUserRepository = GetHttpUserRepository();

        var result = await httpUserRepository.AuthenticateUser(userId, password);

        Assert.False(result);
    }

    [Theory]
    [AutoTestData]
    public async Task CreateUser_ReturnsUser_WhenUserIsCreated(CreateUserRequest request)
    {
        var user = new User { UserId = request.UserId, Password = request.Password };

        SetupHttpMessageHandler(HttpStatusCode.OK, user);

        var httpUserRepository = GetHttpUserRepository();

        var result = await httpUserRepository.CreateUser(request);

        Assert.Equal(user.UserId, result.UserId);
        Assert.Equal(user.Password, result.Password);
    }
    
    [Theory]
    [AutoTestData]
    public async Task UpdateUserPassword_ReturnsUser_WhenUserIsUpdated(UpdateUserPasswordRequest request)
    {
        var user = new User { UserId = request.UserId, Password = request.NewPassword };

        SetupHttpMessageHandler(HttpStatusCode.OK, user);

        var httpUserRepository = GetHttpUserRepository();

        var result = await httpUserRepository.UpdateUserPassword(request);

        Assert.Equal(user.Password, result.Password);
    }
    
    [Theory]
    [AutoTestData]
    public async Task DeleteUser_ReturnsUser_WhenUserIsDeleted(string userId, string password)
    {
        var user = new User { UserId = userId, Password = password };

        SetupHttpMessageHandler(HttpStatusCode.OK, user);

        var httpUserRepository = GetHttpUserRepository();

        var result = await httpUserRepository.DeleteUser(userId, password);

        Assert.Equal(user.UserId, result.UserId);
        Assert.Equal(user.Password, result.Password);
    }

    private HttpUserRepository GetHttpUserRepository() => new(_httpClient, _config);

    private HttpResponseMessage GetHttpResponseMessage(HttpStatusCode statusCode, object content)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(JsonSerializer.Serialize(content))
        };
    }

    private void SetupHttpMessageHandler(HttpStatusCode statusCode, object content)
    {
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(GetHttpResponseMessage(statusCode, content));
    }
}
