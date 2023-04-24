using AppSettingsManagerBff.Infrastructure.ApiRepositories;
using System.Net;
using System.Text.Json;
using AppSettingsManagerBff.Model;
using AppSettingsManagerBff.Model.Requests;
using AutoFixture;
using Moq.Protected;

namespace AppSettingsManagerBff.Infrastructure.Tests;

public class HttpSettingsRepositoryTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();

    private readonly AppSettingsManagerApiConfig _config = new AppSettingsManagerApiConfig
    {
        BaseAddress = "http://localhost:5000/"
    };
    
    private readonly HttpClient _httpClient;
    private readonly Fixture _fixture;
    
    public HttpSettingsRepositoryTests()
    {
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        
        _httpClient.BaseAddress = new Uri(_config.BaseAddress + "settings/");
        
        _fixture = FixtureBuilder.BuildFixture();
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_Succeeds(string userId, string password, string settingGroupId)
    {
        var settingGroup = _fixture.Create<SettingGroup>();
        
        SetupHttpMessageHandler(HttpStatusCode.OK, settingGroup);
        
        var httpSettingsRepository = GetHttpSettingsRepository();
        
        var result = await httpSettingsRepository.GetSettingGroup(userId, password, settingGroupId);
        
        Assert.Equal(settingGroup.Id, result.Id);
    }
    
    [Theory]
    [AutoTestData]
    public async Task GetSettingGroupsForUser_Success(string userId, string password)
    {
        var settingGroups = _fixture.CreateMany<SettingGroup>();
        
        SetupHttpMessageHandler(HttpStatusCode.OK, settingGroups);
        
        var httpSettingsRepository = GetHttpSettingsRepository();
        
        var result = await httpSettingsRepository.GetSettingGroupsForUser(userId, password);
        
        Assert.True(result.Select(sg => sg.Id).OrderBy(sg => sg).SequenceEqual(settingGroups.Select(sg => sg.Id).OrderBy(sg => sg)));
    }
    
    [Theory]
    [AutoTestData]
    public async Task CreateSettingGroup_Succeeds(string settingGroupId, string userId, string password, Dictionary<string, string> input)
    {
        var settingGroup = _fixture.Create<SettingGroup>();
        
        SetupHttpMessageHandler(HttpStatusCode.OK, settingGroup);
        
        var httpSettingsRepository = GetHttpSettingsRepository();

        var createSettingRequest = new CreateSettingRequest
        {
            SettingGroupId = settingGroupId,
            Input = JsonSerializer.SerializeToNode(input),
            UserId = userId,
            Password = password
        };
        
        var result = await httpSettingsRepository.CreateSettingGroup(createSettingRequest);
        
        Assert.Equal(settingGroup.Id, result.Id);
    }

    [Theory]
    [AutoTestData]
    public async Task UpdateSetting_Succeeds(string settingGroupId, string userId, string password,
        Dictionary<string, string> input)
    {
        var settingGroup = _fixture.Create<SettingGroup>();
        
        SetupHttpMessageHandler(HttpStatusCode.OK, settingGroup);
        
        var httpSettingsRepository = GetHttpSettingsRepository();
        
        var updateSettingRequest = new CreateSettingRequest
        {
            SettingGroupId = settingGroupId,
            Input = JsonSerializer.SerializeToNode(input),
            UserId = userId,
            Password = password
        };
        
        var result = await httpSettingsRepository.UpdateSetting(updateSettingRequest);
        
        Assert.Equal(settingGroup.Id, result.Id);
    }

    [Theory]
    [AutoTestData]
    public async Task ChangeTargetSettingVersion_Succeeds(UpdateTargetSettingRequest request)
    {
        var settingGroup = _fixture.Create<SettingGroup>();
        
        SetupHttpMessageHandler(HttpStatusCode.OK, settingGroup);
        
        var httpSettingsRepository = GetHttpSettingsRepository();
        
        var result = await httpSettingsRepository.ChangeTargetSettingVersion(request);
        
        Assert.Equal(settingGroup.Id, result.Id);
    }
    
    [Theory]
    [AutoTestData]
    public async Task DeleteSettingGroup_Succeeds(string userId, string password, string settingGroupId)
    {
        var settingGroup = _fixture.Create<SettingGroup>();
        
        SetupHttpMessageHandler(HttpStatusCode.OK, settingGroup);
        
        var httpSettingsRepository = GetHttpSettingsRepository();
        
        var result = await httpSettingsRepository.DeleteSettingGroup(userId, password, settingGroupId);
        
        Assert.Equal(settingGroup.Id, result.Id);
    }

    [Theory]
    [AutoTestData]
    public async Task UpdatePermission_Succeeds(UpdatePermissionRequest request)
    {
        var permission = _fixture.Create<Permission>();
        
        SetupHttpMessageHandler(HttpStatusCode.OK, permission);
        
        var httpSettingsRepository = GetHttpSettingsRepository();
        
        var result = await httpSettingsRepository.UpdatePermission(request);

        Assert.Equal(permission.SettingGroupId, result.SettingGroupId);
        Assert.Equal(permission.UserId, result.UserId);
    }

    [Theory]
    [AutoTestData]
    public async Task PermissionRequestResponse_Succeeds(PermissionRequestResponse request)
    {
        var permission = _fixture.Create<Permission>();
        
        SetupHttpMessageHandler(HttpStatusCode.OK, permission);
        
        var httpSettingsRepository = GetHttpSettingsRepository();
        
        var result = await httpSettingsRepository.PermissionRequestResponse(request);
        
        Assert.Equal(permission.SettingGroupId, result.SettingGroupId);
    }

    private HttpSettingsRepository GetHttpSettingsRepository() => new (_httpClient, _config);
    
    private HttpResponseMessage GetHttpResponseMessage(HttpStatusCode statusCode, object content)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
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