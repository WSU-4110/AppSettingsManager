using AppSettingsManagerBff.Model;
using AppSettingsManagerBff.Model.Requests;

namespace AppSettingsManagerBff.Domain.ApiRepositories;

public interface IUserRepository
{
    Task<bool> AuthenticateUser(string userId, string password);
    Task<User> CreateUser(CreateUserRequest request);
    Task<User> UpdateUserPassword(UpdateUserPasswordRequest request);
    Task<User> DeleteUser(string userId, string password);
}