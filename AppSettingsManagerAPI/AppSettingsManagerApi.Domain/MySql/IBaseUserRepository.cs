using AppSettingsManagerApi.Model;

namespace AppSettingsManagerApi.Domain.MySql;

public interface IBaseUserRepository
{
    Task<BaseUser> CreateUser(string userId, string password);
}