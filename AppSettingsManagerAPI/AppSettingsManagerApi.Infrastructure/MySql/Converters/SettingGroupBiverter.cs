using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class SettingGroupBiverter : IBidirectionalConverter<Model.SettingGroup, SettingGroup>
{
    private readonly IBidirectionalConverter<Model.Setting, Setting> _settingVersionBiverter;
    private readonly IBidirectionalConverter<Model.Permission, Permission> _permissionsBiverter;

    public SettingGroupBiverter(
        IBidirectionalConverter<Model.Setting, Setting> settingVersionBiverter,
        IBidirectionalConverter<Model.Permission, Permission> permissionsBiverter
    )
    {
        _settingVersionBiverter = settingVersionBiverter;
        _permissionsBiverter = permissionsBiverter;
    }

    public SettingGroup Convert(Model.SettingGroup source)
    {
        return new SettingGroup
        {
            SettingGroupId = source.SettingId,
            CreatedBy = source.CreatedBy,
            Settings = source.Settings.Select(_settingVersionBiverter.Convert).ToList(),
            Permissions = source.Permissions.Select(_permissionsBiverter.Convert).ToList()
        };
    }

    public Model.SettingGroup Convert(SettingGroup source)
    {
        return new Model.SettingGroup
        {
            SettingId = source.SettingGroupId,
            CreatedBy = source.CreatedBy,
            Settings = source.Settings.Select(_settingVersionBiverter.Convert),
            Permissions = source.Permissions.Select(_permissionsBiverter.Convert)
        };
    }
}
