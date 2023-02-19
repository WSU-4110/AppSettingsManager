using System.ComponentModel.DataAnnotations;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class BaseUser
{
    // These labels are optional but recommended
    // Make sure the limits you set make sense based on the field being affected
    [MaxLength(36)]
    public string UserId { get; set; }

    [MaxLength(36)]
    public string Password { get; set; }

    [MaxLength(50)]
    public string Email { get; set; }

    // The BaseUser has many Settings relationship is defined in the SettingsContext.cs file
    public List<Setting> Settings { get; set; }
}
