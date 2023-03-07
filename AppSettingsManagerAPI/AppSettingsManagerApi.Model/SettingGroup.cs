using System.Collections;

namespace AppSettingsManagerApi.Model;

public class SettingGroup
{
    public string SettingId { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
    public IEnumerable<Setting> Settings { get; set; }
    public IEnumerable<Permission> Permissions { get; set; }
}
