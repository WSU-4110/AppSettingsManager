using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model;
using AppSettingsManagerBff.Model.Requests;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public class HttpUserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;

    public HttpUserRepository(HttpClient httpClient, AppSettingsManagerApiConfig config)
    {
        _httpClient = httpClient;

        // So you don't have to type the whole url every time
        _httpClient.BaseAddress = new Uri(config.BaseAddress + "users/");
    }

    public async Task<bool> AuthenticateUser(string userId, string password)
    {
        var response = await _httpClient.GetAsync($"auth/userId/{userId}/password/{password}");
        return await Constants.DeserializeResponse<bool>(response);
    }

    public async Task<User> CreateUser(CreateUserRequest request)
    {
        var content = Constants.GetStringContent(request);

        var response = await _httpClient.PostAsync("", content);

        return await Constants.DeserializeResponse<User>(response);
    }

    public async Task<User> UpdateUserPassword(UpdateUserPasswordRequest request)
    {
        var content = Constants.GetStringContent(request);

        var response = await _httpClient.PutAsync("", content);

        return await Constants.DeserializeResponse<User>(response);
    }

    public async Task<User> DeleteUser(string userId, string password)
    {
        var response = await _httpClient.DeleteAsync(Constants.GetRequestUri(userId, password));
        return await Constants.DeserializeResponse<User>(response);
    }
}