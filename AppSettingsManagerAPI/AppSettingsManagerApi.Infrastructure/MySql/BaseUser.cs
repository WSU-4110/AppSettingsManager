using System.ComponentModel.DataAnnotations;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class BaseUser
{
    [MaxLength(36)]
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
