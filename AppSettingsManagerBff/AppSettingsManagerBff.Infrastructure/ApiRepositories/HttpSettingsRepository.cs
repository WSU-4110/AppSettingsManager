using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model.Api;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public class HttpSettingsRepository : ISettingsRepository
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    // See comments on HttpUserRepository for explanations on this set up
    public HttpSettingsRepository(HttpClient httpClient, AppSettingsManagerApiConfig config)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(config.BaseAddress + "settings/");
    }

    public async Task<ApiSetting> CreateSetting(CreateSettingRequest request)
    {
        // Serialize request object to string
        var serializedRequest = JsonSerializer.Serialize(request);

        // Wrap request object in StringContent object
        var content = new StringContent(
            serializedRequest,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // Send request
        // The requestUri parameter is "" because there is nothing to add to the base URL we have already set up
        var response = await _httpClient.PostAsync("", content);
        response.EnsureSuccessStatusCode();

        // Extract response content as json string
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserialize json string
        var setting = JsonSerializer.Deserialize<ApiSetting>(jsonResponse, _jsonSerializerOptions);

        // "??" means "if null then"
        return setting ?? throw new HttpRequestException();
    }

    public async Task<ApiSetting> GetSetting([FromRoute][Required] string settingId, [FromRoute][Required] int version)
    {
        // Send request
        var response = await _httpClient.GetAsync($"settingId/{settingId}/version/{version}");
        response.EnsureSuccessStatusCode();

        // Extract response content as json string
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserialize json string
        var setting = JsonSerializer.Deserialize<ApiSetting>(jsonResponse, _jsonSerializerOptions);

        // "??" means "if null then"
        return setting ?? throw new HttpRequestException();
    }

    public async Task<ApiSetting> UpdateSetting([FromRoute][Required] UpdateSettingRequest request)
    {
        // Serialize request object to string
        var serializedRequest = JsonSerializer.Serialize(request);

        // Wrap request object in StringContent object
        var content = new StringContent(
            serializedRequest,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // Send request
        var response = await _httpClient.PutAsync(string.Empty(), content);      
        response.EnsureSuccessStatusCode();

        // Extract response content as json string
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserialize json string
        var setting = JsonSerializer.Deserialize<ApiSetting>(jsonResponse, _jsonSerializerOptions);

        // "??" means "if null then"
        return setting ?? throw new HttpRequestException();
    }

    public async Task<ApiSetting> DeleteSetting([FromRoute][Required] string settingId)
    {
        // Send request
        var response = await _httpClient.DeleteAsync($"{settingId}");
        response.EnsureSuccessStatusCode();

        // Extract response content as json string
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserialize json string
        var setting = JsonSerializer.Deserialize<ApiSetting>(jsonResponse, _jsonSerializerOptions);

        // "??" means "if null then"
        return setting ?? throw new HttpRequestException();
    }
}