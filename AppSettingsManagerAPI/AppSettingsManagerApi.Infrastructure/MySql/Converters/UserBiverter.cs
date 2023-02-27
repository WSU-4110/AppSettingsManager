using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

/// <summary>
/// It's useful to have some standardized converters for objects that you'll be switching between a lot
/// </summary>
public class UserBiverter : IBidirectionalConverter<Model.User, User>
{
    private readonly IBidirectionalConverter<Model.Permission, Permission> _permissionsBiverter;

    public UserBiverter(IBidirectionalConverter<Model.Permission, Permission> permissionsBiverter)
    {
        _permissionsBiverter = permissionsBiverter;
    }

    public User Convert(Model.User source)
    {
        return new User
        {
            UserId = source.UserId,
            Password = source.Password,
            Email = source.Email,
            Permissions =
                source.Permissions?.Select(_permissionsBiverter.Convert).ToList()
                ?? new List<Permission>()
        };
    }

    public Model.User Convert(User source)
    {
        return new Model.User
        {
            UserId = source.UserId,
            Password = source.Password,
            Email = source.Email,
            Permissions =
                source.Permissions?.Select(_permissionsBiverter.Convert).ToList()
                ?? new List<Model.Permission>()
        };
    }
}
