namespace AppSettingsManagerApi.Model.Requests;

public class GetSettingGroupRequest
{
    public string SettingGroupId { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
}
