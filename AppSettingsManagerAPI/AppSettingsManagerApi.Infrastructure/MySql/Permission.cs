using System.ComponentModel.DataAnnotations.Schema;
using AppSettingsManagerApi.Model;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class Permission
{
    [ForeignKey("User")]
    public string UserId { get; set; }
    public User User { get; set; }

    [ForeignKey("SettingGroup")]
    public string SettingGroupId { get; set; }
    public SettingGroup SettingGroup { get; set; }
    public PermissionLevel CurrentPermissionLevel { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public bool WaitingForApproval { get; set; }
    public PermissionLevel RequestedPermissionLevel { get; set; } = PermissionLevel.None;
}
