namespace AppSettingsManagerBff.Model.Requests;

public class UpdateTargetSettingRequest
{
    public string SettingGroupId { get; set; }
    public int NewCurrentVersion { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
}