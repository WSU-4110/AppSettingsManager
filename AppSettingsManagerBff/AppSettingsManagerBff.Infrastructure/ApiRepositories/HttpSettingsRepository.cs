using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model;
using AppSettingsManagerBff.Model.Requests;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public class HttpSettingsRepository : ISettingsRepository
{
    private readonly HttpClient _httpClient;

    // See comments on HttpUserRepository for explanations on this set up
    public HttpSettingsRepository(HttpClient httpClient, AppSettingsManagerApiConfig config)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(config.BaseAddress + "settings/");
    }

    public async Task<SettingGroup> GetSettingGroup(
        string userId,
        string password,
        string settingGroupId
    )
    {
        var response = await _httpClient.GetAsync(
            Constants.GetRequestUri(userId, password, settingGroupId)
        );

        return await Constants.DeserializeResponse<SettingGroup>(response);
    }

    public async Task<IEnumerable<SettingGroup>> GetSettingGroupsForUser(
        string userId,
        string password
    )
    {
        var response = await _httpClient.GetAsync(Constants.GetRequestUri(userId, password));

        return await Constants.DeserializeResponse<IEnumerable<SettingGroup>>(response);
    }

    public async Task<SettingGroup> CreateSettingGroup(CreateSettingRequest request)
    {
        var content = Constants.GetStringContent(request);
        var response = await _httpClient.PostAsync("", content);

        return await Constants.DeserializeResponse<SettingGroup>(response);
    }

    public async Task<SettingGroup> UpdateSetting(CreateSettingRequest request)
    {
        var content = Constants.GetStringContent(request);
        var response = await _httpClient.PutAsync("", content);

        return await Constants.DeserializeResponse<SettingGroup>(response);
    }

    public async Task<SettingGroup> ChangeTargetSettingVersion(UpdateTargetSettingRequest request)
    {
        var content = Constants.GetStringContent(request);
        var response = await _httpClient.PutAsync("target", content);

        return await Constants.DeserializeResponse<SettingGroup>(response);
    }

    public async Task<SettingGroup> DeleteSettingGroup(
        string userId,
        string password,
        string settingGroupId
    )
    {
        var response = await _httpClient.DeleteAsync(
            Constants.GetRequestUri(userId, password, settingGroupId)
        );

        return await Constants.DeserializeResponse<SettingGroup>(response);
    }
}