using AppSettingsManagerApi.Domain.Conversion;
using MySqlBaseUser = AppSettingsManagerApi.Infrastructure.MySql.BaseUser;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

/// <summary>
/// It's useful to have some standardized converters for objects that you'll be switching between a lot
/// </summary>
public class BaseUserBiverter : IBidirectionalConverter<Model.BaseUser, MySqlBaseUser>
{
    private readonly IBidirectionalConverter<Model.Setting, Setting> _settingConverter;

    public BaseUserBiverter(IBidirectionalConverter<Model.Setting, Setting> settingConverter)
    {
        _settingConverter = settingConverter;
    }

    public MySqlBaseUser Convert(Model.BaseUser source)
    {
        return new MySqlBaseUser
        {
            UserId = source.UserId, 
            Password = source.Password,
            Settings = source.Settings.Select(_settingConverter.Convert).ToList()
        };
    }

    public Model.BaseUser Convert(MySqlBaseUser source)
    {
        return new Model.BaseUser
        {
            UserId = source.UserId, 
            Password = source.Password,
            Settings = source.Settings.Select(_settingConverter.Convert).ToList()
        };
    }
}
