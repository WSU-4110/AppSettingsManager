using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.Exceptions;
using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Model.Requests;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class MySqlUserRepository : IUserRepository
{
    private readonly SettingsContext _settingsContext;
    private readonly IBidirectionalConverter<Model.User, User> _userConverter;

    public MySqlUserRepository(
        SettingsContext settingsContext,
        IBidirectionalConverter<Model.User, User> userConverter
    )
    {
        _settingsContext = settingsContext;
        _userConverter = userConverter;
    }

    public async Task<bool> AuthenticateUser(string userId, string password)
    {
        try
        {
            var user = await _settingsContext.Users.SingleAsync(
                u => u.Id == userId && u.Password == password
            );

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<Model.User> CreateUser(CreateUserRequest request)
    {
        var user = new User
        {
            Id = request.UserId,
            Password = request.Password,
            Email = request.Email
        };

        _settingsContext.Users.Add(user);
        await _settingsContext.SaveChangesAsync();

        return _userConverter.Convert(user);
    }

    public async Task<Model.User> UpdateUserPassword(UpdateUserPasswordRequest request)
    {
        var user = await _settingsContext.Users.SingleAsync(u => u.Id == request.UserId);

        CheckPassword(user, request.OldPassword);

        user.Password = request.NewPassword;

        await _settingsContext.SaveChangesAsync();

        return _userConverter.Convert(user);
    }

    public async Task<Model.User> DeleteUser(DeleteUserRequest request)
    {
        var user = await _settingsContext.Users.SingleAsync(u => u.Id == request.UserId);

        CheckPassword(user, request.Password);

        _settingsContext.Users.Remove(user);
        await _settingsContext.SaveChangesAsync();

        return _userConverter.Convert(user);
    }

    private void CheckPassword(User user, string password)
    {
        if (user.Password != password)
        {
            throw new IncorrectPasswordException(user.Id);
        }
    }
}
