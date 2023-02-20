using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.MySql;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class MySqlBaseUserRepository : IBaseUserRepository
{
    private readonly SettingsContext _settingsContext;
    private readonly IBidirectionalConverter<Model.BaseUser, BaseUser> _baseUserConverter;

    public MySqlBaseUserRepository(
        SettingsContext settingsContext,
        IBidirectionalConverter<Model.BaseUser, BaseUser> baseUserConverter
    )
    {
        _settingsContext = settingsContext;
        _baseUserConverter = baseUserConverter;
    }

    public async Task<Model.BaseUser> GetUser(string userId)
    {
        return _baseUserConverter.Convert(
            await _settingsContext.BaseUsers.SingleAsync(u => u.UserId == userId)
        );
    }

    public async Task<Model.BaseUser> CreateUser(string userId, string password, string email)
    {
        var newUser = new BaseUser { UserId = userId, Password = password, Email = email };

        await _settingsContext.BaseUsers.AddAsync(newUser);

        await _settingsContext.SaveChangesAsync();

        return _baseUserConverter.Convert(
            _settingsContext.BaseUsers.Single(u => u.UserId == userId)
        );
    }
    public async Task<Model.BaseUser> DeleteUser(string userId)
    {
        var user = await _settingsContext.BaseUsers.SingleAsync(u => u.UserId == userId);

    _settingsContext.BaseUsers.Remove(user);
    await _settingsContext.SaveChangesAsync();
    return _baseUserConverter.Convert(user);
    }
    public async Task<Model.BaseUser> UpdateUser(string userId, string newPassword)
    {
        var user = await _settingsContext.BaseUsers.SingleOrDefaultAsync(u => u.UserId == userId);

        if (user != null)
        {
            user.Password = newPassword;
            await _settingsContext.SaveChangesAsync();
            return _baseUserConverter.Convert(user);
        }
        else
        {
            return null;
        }
    }
}
