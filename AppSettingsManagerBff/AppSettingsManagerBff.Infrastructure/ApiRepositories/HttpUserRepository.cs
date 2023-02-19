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

        // So you don't have to type the whole url every time
        _httpClient.BaseAddress = new Uri(config.BaseAddress + "users/");
    }

    public async Task<ApiBaseUser> GetUser(string userId)
    {
        var response = await _httpClient.GetAsync($"userId/{userId}");

        // Will throw exception if status code not in the 200s, meaning request failed
        response.EnsureSuccessStatusCode();

        // The response content will be in a json string format. Can extract to string like so
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Have to deserialize the string to an object, in this case it is ApiBaseUser
        var user = JsonSerializer.Deserialize<ApiBaseUser>(jsonResponse, _jsonSerializerOptions);

        // The "??" here means "if null then"
        return user ?? throw new HttpRequestException();
    }

    public async Task<ApiBaseUser> CreateUser(string userId, string password)
    {
        // this is just a way to suppress errors from there being no return value on this method
        throw new NotImplementedException();
    }
}