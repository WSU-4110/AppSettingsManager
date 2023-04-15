using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

/// <summary>
/// It's useful to have some standardized converters for objects that you'll be switching between a lot
/// </summary>
public class UserConverter : IBidirectionalConverter<Model.User, User>
{
    public User Convert(Model.User source)
    {
        return new User
        {
            Id = source.UserId,
            Password = source.Password,
            Email = source.Email
        };
    }

    public Model.User Convert(User source)
    {
        return new Model.User
        {
            UserId = source.Id,
            Password = source.Password,
            Email = source.Email
        };
    }
}
