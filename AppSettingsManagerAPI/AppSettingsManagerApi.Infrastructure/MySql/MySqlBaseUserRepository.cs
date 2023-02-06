using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.MySql;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class MySqlBaseUserRepository : IBaseUserRepository
{
    private readonly SettingsContext _settingsContext;
    private readonly IBidirectionalConverter<Model.BaseUser, BaseUser> _baseUserConverter;

    public MySqlBaseUserRepository(SettingsContext settingsContext, IBidirectionalConverter<Model.BaseUser, BaseUser> baseUserConverter)
    {
        _settingsContext = settingsContext;
        _baseUserConverter = baseUserConverter;
    }

    public async Task<Model.BaseUser> CreateUser(string userId, string password)
    {
        var newUser = new BaseUser
        {
            UserId = userId,
            Password = password
        };
        
        await _settingsContext.BaseUsers.AddAsync(newUser);

        return _baseUserConverter.Convert(_settingsContext.BaseUsers.Single(u => u.UserId == userId));
    }
}