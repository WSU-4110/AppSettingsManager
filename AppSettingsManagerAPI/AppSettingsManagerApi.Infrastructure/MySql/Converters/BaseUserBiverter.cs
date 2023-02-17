using AppSettingsManagerApi.Domain.Conversion;
using MySqlBaseUser = AppSettingsManagerApi.Infrastructure.MySql.BaseUser;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

/// <summary>
/// It's useful to have some standardized converters for objects that you'll be switching between a lot
/// </summary>
public class BaseUserBiverter : IBidirectionalConverter<Model.BaseUser, MySqlBaseUser>
{
    public MySqlBaseUser Convert(Model.BaseUser input)
    {
        return new MySqlBaseUser { UserId = input.UserId, Password = input.Password };
    }

    public Model.BaseUser Convert(MySqlBaseUser input)
    {
        return new Model.BaseUser { UserId = input.UserId, Password = input.Password };
    }
}
