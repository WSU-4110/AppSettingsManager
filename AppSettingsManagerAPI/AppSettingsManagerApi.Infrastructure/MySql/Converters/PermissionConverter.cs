using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class PermissionConverter : IBidirectionalConverter<Model.Permission, Permission>
{
    public Permission Convert(Model.Permission source)
    {
        return new Permission
        {
            UserId = source.UserId,
            SettingGroupId = source.SettingGroupId,
            CurrentPermissionLevel = source.CurrentPermissionLevel,
            ApprovedBy = source.ApprovedBy,
            RequestedPermissionLevel = source.RequestedPermissionLevel,
            WaitingForApproval = source.WaitingForApproval
        };
    }

    public Model.Permission Convert(Permission source)
    {
        return new Model.Permission
        {
            UserId = source.UserId,
            SettingGroupId = source.SettingGroupId,
            ApprovedBy = source.ApprovedBy,
            CurrentPermissionLevel = source.CurrentPermissionLevel,
            RequestedPermissionLevel = source.RequestedPermissionLevel,
            WaitingForApproval = source.WaitingForApproval
        };
    }
}
