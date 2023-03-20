using System.Text.Json;
using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model.Api;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public class HttpUserRepository : IUserRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpUserRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient(Constants.UsersClientName);
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

    public async Task<ApiBaseUser> CreateUser(string userId, string password, string email)
    {
        // this is just a way to suppress errors from there being no return value on this method
        var response = await _httpClient.PostAsync(
            $"userId/{userId}/password/{password}/email/{email}",
            new StringContent(string.Empty)
        );
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<ApiBaseUser>(jsonResponse, _jsonSerializerOptions);

        return user ?? throw new HttpRequestException();
    }

    public async Task<ApiBaseUser> UpdateUser(string userId, string newPassword)
    {
        //Send Request
        var response = await _httpClient.PutAsync(
            $"userId/{userId}/password/{newPassword}",
            new StringContent(string.Empty)
        );

        response.EnsureSuccessStatusCode();

        //Extract response content as a json string
        var jsonResponse = await response.Content.ReadAsStringAsync();

        //Deserialize json string
        var user = JsonSerializer.Deserialize<ApiBaseUser>(jsonResponse, _jsonSerializerOptions);

        return user ?? throw new HttpRequestException();
    }

    public async Task<ApiBaseUser> DeleteUser(string userId)
    {
        //Sending request
        var response = await _httpClient.DeleteAsync($"delete/userId/{userId}");
        response.EnsureSuccessStatusCode();

        //Extract response content as json string
        var jsonResponse = await response.Content.ReadAsStringAsync();

        //Deserialize json string
        var user = JsonSerializer.Deserialize<ApiBaseUser>(jsonResponse, _jsonSerializerOptions);

        return user ?? throw new HttpRequestException();
    }
}
