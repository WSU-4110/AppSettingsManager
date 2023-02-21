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

    public async Task<ApiBaseUser> UpdateUser([FromRoute][Required] UpdateUserRequest request)
    {
        //Serialize request object to string
        var serializedRequest = JsonSerializer.Serialize(request);

        //Wrap request object in StringContent object
        var content = new StringContent(serializedRequest, System.Text.Encoding.UTF8, "application/json");

        //Send Request
        var response = await _httpClient.PutAsync($"userId/{request.userId}/password{request.password}");
        response.EnsureSuccessStatusCode();


        //Extract response content as a json string
        var jsonResponse = await response.Content.ReadAsStringAsync();


        //Deserialize json string
        var user = JsonSerializer.Deserialize<ApiBaseUser>(jsonResponse, _jsonSerializerOptions);
 
 
        return user ?? throw new HttpRequestException();
    }

public async Task<ApiBaseUser> DeleteUser([FromRoute][Required] string userId)
{

    //Sending request
    var response = await _httpClient.DeleteUser($"userId/{userId}");
    response.EnsureSuccessStatusCode();


    //Extract response content as json string
    var jsonResponse = await response.Content.ReadAsStringAsync();


    //Deserialize json string
    var user = JsonSerializer.Deserialize<ApiBaseUser>(jsonResponse, _jsonSerializerOptions);

    return user ?? throw new HttpRequestException();
}
}