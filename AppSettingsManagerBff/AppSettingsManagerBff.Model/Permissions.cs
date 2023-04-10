namespace AppSettingsManagerBff.Model;

public class Permission
{
    public string UserId { get; set; }
    public string SettingGroupId { get; set; }
    public PermissionLevel CurrentPermissionLevel { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public bool WaitingForApproval { get; set; } = false;
    public PermissionLevel RequestedPermissionLevel { get; set; }
}