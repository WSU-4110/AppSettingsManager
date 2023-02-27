using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.MySql;
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

    public async Task<Model.User> GetUser(string userId)
    {
        return _userConverter.Convert(
            await _settingsContext.Users.SingleAsync(u => u.UserId == userId)
        );
    }

    public async Task<Model.User> CreateUser(string userId, string password, string email)
    {
        var newUser = new User
        {
            UserId = userId,
            Password = password,
            Email = email
        };

        await _settingsContext.Users.AddAsync(newUser);

        await _settingsContext.SaveChangesAsync();

        return _userConverter.Convert(_settingsContext.Users.Single(u => u.UserId == userId));
    }

    public async Task<Model.User> DeleteUser(string userId)
    {
        var user = await _settingsContext.Users.SingleAsync(u => u.UserId == userId);

        _settingsContext.Users.Remove(user);
        await _settingsContext.SaveChangesAsync();
        return _userConverter.Convert(user);
    }

    public async Task<Model.User> UpdateUser(string userId, string newPassword)
    {
        var user = await _settingsContext.Users.SingleAsync(u => u.UserId == userId);

        user.Password = newPassword;
        await _settingsContext.SaveChangesAsync();
        return _userConverter.Convert(user);
    }
}
