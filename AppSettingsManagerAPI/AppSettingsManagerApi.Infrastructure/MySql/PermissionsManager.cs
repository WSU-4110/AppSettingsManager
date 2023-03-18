// The PermissionsManager class implements all the methods needed to 
// check whether the user has the necessary permissions to access a settingGroup
// and allows the user to request permission to a settingGroup if already don't have it
// The user (owner of the project or admin) can then approve or reject the request
public class PermissionsManager
{
    private readonly List<Permission> _permissions;

    public PermissionsManager()
    {
        _permissions = new List<Permission>();
    }

    public void AddPermission(Permission permission)
    {
        _permissions.Add(permission);
    }

    public bool CanUserAccessSetting(User user, SettingGroup settingGroup, PermissionLevel requiredPermissionLevel)
    {
        var permission = _permissions.FirstOrDefault(p => p.UserId == user.Id && p.SettingGroupId == settingGroup.SettingGroupId);

        if (permission == null)
        {
            return false;
        }

        if (permission.CurrentPermissionLevel >= requiredPermissionLevel)
        {
            return true;
        }

        return false;
    }

    public void RequestPermission(User user, SettingGroup settingGroup, PermissionLevel requestedPermissionLevel)
    {
        var permission = _permissions.FirstOrDefault(p => p.UserId == user.Id && p.SettingGroupId == settingGroup.SettingGroupId);

        if (permission != null)
        {
            permission.RequestedPermissionLevel = requestedPermissionLevel;
            permission.WaitingForApproval = true;
        }
        else
        {
            var newPermission = new PermissionObject
            {
                UserId = user.Id,
                User = user,
                SettingGroupId = settingGroup.SettingGroupId,
                SettingGroup = settingGroup,
                CurrentPermissionLevel = PermissionLevel.None,
                RequestedPermissionLevel = requestedPermissionLevel,
                WaitingForApproval = true
            };

            _permissions.Add(newPermission);
        }
    }

    public void ApprovePermission(User approvingUser, PermissionObject permission)
    {
        permission.ApprovedBy = approvingUser.Id;
        permission.CurrentPermissionLevel = permission.RequestedPermissionLevel;
        permission.WaitingForApproval = false;
    }

    public void RejectPermission(User rejectingUser, PermissionObject permission)
    {
        permission.ApprovedBy = rejectingUser.Id;
        permission.RequestedPermissionLevel = permission.CurrentPermissionLevel;
        permission.WaitingForApproval = false;
    }
}
