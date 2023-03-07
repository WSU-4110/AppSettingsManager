using AppSettingsManagerApi.Model;

namespace AppSettingsManagerApi.Domain.MySql;

public interface IUserRepository
{
    Task<User> GetUser(string userId);
    Task<User> CreateUser(string userId, string password, string email);
    Task<User> DeleteUser(string userId);
    Task<User> UpdateUser(string userId, string newPassword);
}
