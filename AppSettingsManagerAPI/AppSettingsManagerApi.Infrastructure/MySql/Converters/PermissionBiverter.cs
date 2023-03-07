using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class PermissionBiverter : IBidirectionalConverter<Model.Permission, Permission>
{
    private readonly IBidirectionalConverter<Model.User, User> _userBiverter;

    private readonly IBidirectionalConverter<
        Model.SettingGroup,
        SettingGroup
    > _settingGroupBiverter;

    public PermissionBiverter(
        IBidirectionalConverter<Model.User, User> userBiverter,
        IBidirectionalConverter<Model.SettingGroup, SettingGroup> settingGroupBiverter
    )
    {
        _userBiverter = userBiverter;
        _settingGroupBiverter = settingGroupBiverter;
    }

    public Permission Convert(Model.Permission source)
    {
        return new Permission
        {
            UserId = source.UserId,
            User = _userBiverter.Convert(source.User),
            SettingGroupId = source.SettingGroupId,
            SettingGroup = _settingGroupBiverter.Convert(source.SettingGroup),
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
            User = _userBiverter.Convert(source.User),
            SettingGroupId = source.SettingGroupId,
            SettingGroup = _settingGroupBiverter.Convert(source.SettingGroup),
            ApprovedBy = source.ApprovedBy,
            CurrentPermissionLevel = source.CurrentPermissionLevel,
            RequestedPermissionLevel = source.RequestedPermissionLevel,
            WaitingForApproval = source.WaitingForApproval
        };
    }
}
