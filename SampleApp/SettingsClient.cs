namespace SampleApp;

public class SettingsClient
{
    private readonly HttpClient _httpClient;

    private const string AppSettingsManagerUsername = "jack";
    private const string AppSettingsManagerPassword = "pwd";
    private const string SettingGroupId = "sample_app";
    private const string BaseAddress =
        "https://appsettingsmanagerapi.azurewebsites.net/settings/setting";

    public SettingsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(
            $"{BaseAddress}/userId/{AppSettingsManagerUsername}/password/{AppSettingsManagerPassword}/settingGroupId/{SettingGroupId}/"
        );
    }

    public async Task<Dictionary<string, string>> GetSettings()
    {
        var response = await _httpClient.GetAsync("");
        var settings = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return settings ?? new Dictionary<string, string>();
    }

    public async Task<string> GetSetting(string settingName)
    {
        var response = await _httpClient.GetAsync(settingName);
        var setting = await response.Content.ReadAsStringAsync();
        return setting;
    }
}
