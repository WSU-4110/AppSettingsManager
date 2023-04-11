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

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_Succeeds(string userId, string password, string settingGroupId)
    {
        var settingGroup = new SettingGroup
        {
            Id = settingGroupId,
            CreatedBy = "test",
            LastUpdatedAt = DateTimeOffset.Now
        };

        var expectedResponse = GetHttpResponseMessage(HttpStatusCode.OK, settingGroup);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(expectedResponse);

        var repository = GetHttpSettingsRepository();

        var result = await repository.GetSettingGroup(userId, password, settingGroupId);

        Assert.Equal(settingGroup.Id, result.Id);
    }
    
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