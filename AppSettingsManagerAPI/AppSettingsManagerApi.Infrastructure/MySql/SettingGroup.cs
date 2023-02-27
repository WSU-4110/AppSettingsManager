namespace AppSettingsManagerApi.Infrastructure.MySql;

public class SettingGroup
{
    public string SettingId { get; set; }
    public string CreatedBy { get; set; }
    public List<Setting> Settings { get; set; }
}
