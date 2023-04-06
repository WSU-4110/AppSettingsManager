namespace AppSettingsManagerBff.Model.Requests;

public class UpdatePermissionRequest
{
    public string UserId { get; set; }
    public string SettingGroupId { get; set; }
    public PermissionLevel NewPermissionLevel { get; set; }
}