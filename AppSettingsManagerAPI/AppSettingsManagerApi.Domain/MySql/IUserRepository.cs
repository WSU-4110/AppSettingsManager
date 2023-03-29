using AppSettingsManagerApi.Model;
using AppSettingsManagerApi.Model.Requests;

namespace AppSettingsManagerApi.Domain.MySql;

public interface IUserRepository
{
    Task<bool> AuthenticateUser(string userId, string password);
    Task<User> CreateUser(CreateUserRequest request);
    Task<User> UpdateUserPassword(UpdateUserPasswordRequest request);
    Task<User> DeleteUser(DeleteUserRequest request);
}
