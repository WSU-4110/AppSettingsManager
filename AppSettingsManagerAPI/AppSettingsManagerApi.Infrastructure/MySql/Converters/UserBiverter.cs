using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

/// <summary>
/// It's useful to have some standardized converters for objects that you'll be switching between a lot
/// </summary>
public class UserBiverter : IBidirectionalConverter<Model.User, User>
{
    private readonly IBidirectionalConverter<
        Model.Setting,
        Setting
    > _settingConverter;

    public UserBiverter(
        IBidirectionalConverter<Model.Setting, Setting> settingConverter
    )
    {
        _settingConverter = settingConverter;
    }

    public User Convert(Model.User source)
    {
        return new User
        {
            UserId = source.UserId,
            Password = source.Password,
            Email = source.Email,
            Settings =
                source.Settings?.Select(_settingConverter.Convert).ToList()
                ?? new List<Setting>()
        };
    }

    public Model.User Convert(User source)
    {
        return new Model.User
        {
            UserId = source.UserId,
            Password = source.Password,
            Email = source.Email,
            Settings =
                source.Settings?.Select(_settingConverter.Convert).ToList()
                ?? new List<Model.Setting>()
        };
    }
}
