using System.ComponentModel.DataAnnotations;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class SettingGroup
{
    public string SettingGroupId { get; set; }
    public string CreatedBy { get; set; }

    [Timestamp]
    public byte[] LastUpdatedAt { get; set; }
    public List<Setting> Settings { get; set; }
    public List<Permission> Permissions { get; set; }
}
