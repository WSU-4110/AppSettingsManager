using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model.Api;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public class HttpUserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;

    public HttpUserRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public ApiBaseUser GetUser(string userId)
    {
        throw new NotImplementedException();
    }

    public ApiBaseUser CreateUser(string userId, string password)
    {
        throw new NotImplementedException();
    }
}