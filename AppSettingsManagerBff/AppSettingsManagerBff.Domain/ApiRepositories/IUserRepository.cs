using AppSettingsManagerBff.Model.Api;

namespace AppSettingsManagerBff.Domain.ApiRepositories;

public interface IUserRepository
{
    Task<ApiBaseUser> GetUser(string userId);
    Task<ApiBaseUser> CreateUser(string userId, string password);
}