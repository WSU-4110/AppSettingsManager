namespace AppSettingsManagerApi.Model.Requests;

public class DeleteSettingGroupRequest
{
    public string SettingGroupId { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
}
