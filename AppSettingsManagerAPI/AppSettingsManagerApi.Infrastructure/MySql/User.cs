using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class User
{
    // These labels are optional but recommended
    // Make sure the limits you set make sense based on the field being affected
    [MaxLength(36)]
    public string Id { get; set; }

    [MaxLength(36)]
    public string Password { get; set; }

    [MaxLength(50)]
    public string Email { get; set; }

    // The User has many Permissions relationship is defined in the SettingsContext.cs file
    public List<Permission> Permissions { get; set; } = new List<Permission>();
}
