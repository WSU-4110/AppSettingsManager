namespace AppSettingsManagerApi.Model;

public class SettingGroup
{
    public string SettingId { get; set; }
    public string CreatedBy { get; set; }
    public IEnumerable<SettingVersion> SettingVersions { get; set; }
}
