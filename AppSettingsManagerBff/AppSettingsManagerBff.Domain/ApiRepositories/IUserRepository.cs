using AppSettingsManagerBff.Model.Api;

namespace AppSettingsManagerBff.Domain.ApiRepositories;

public interface IUserRepository
{
    ApiBaseUser GetUser(string userId);
    ApiBaseUser CreateUser(string userId, string password);
}