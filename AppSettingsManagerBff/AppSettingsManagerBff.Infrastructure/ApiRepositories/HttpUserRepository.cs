using System.Text.Json;
using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model.Api;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public class HttpUserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpUserRepository(HttpClient httpClient, AppSettingsManagerApiConfig config)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(config.BaseAddress + "users/");
    }
    
    public async Task<ApiBaseUser> GetUser(string userId)
    {
        var response = await _httpClient.GetAsync($"userId/{userId}");
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        
        var user = JsonSerializer.Deserialize<ApiBaseUser>(jsonResponse, _jsonSerializerOptions);

        return user ?? throw new HttpRequestException();
    }

    public async Task<ApiBaseUser> CreateUser(string userId, string password)
    {
        throw new NotImplementedException();
    }
}