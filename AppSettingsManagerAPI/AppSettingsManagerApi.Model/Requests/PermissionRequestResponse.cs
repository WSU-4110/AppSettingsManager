namespace AppSettingsManagerApi.Model.Requests;

public class PermissionRequestResponse
{
    public string UserId { get; set; }
    public string SettingGroupId { get; set; }
    public bool Approved { get; set; }
    public string ApproverId { get; set; }
    public string Password { get; set; }
}
