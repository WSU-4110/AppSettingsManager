using AppSettingsManagerApi.Model;

namespace AppSettingsManagerApi.Domain.MySql;

public interface IBaseUserRepository
{
    Task<BaseUser> GetUser(string userId);
    Task<BaseUser> CreateUser(string userId, string password);
    Task<BaseUser> DeleteUser(string userId);
}
