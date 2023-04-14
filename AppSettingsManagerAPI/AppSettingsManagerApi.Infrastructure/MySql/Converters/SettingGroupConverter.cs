using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class SettingGroupConverter : IBidirectionalConverter<Model.SettingGroup, SettingGroup>
{
    private readonly IBidirectionalConverter<Model.Setting, Setting> _settingConverter;
    private readonly IBidirectionalConverter<Model.Permission, Permission> _permissionConverter;

    public SettingGroupConverter(
        IBidirectionalConverter<Model.Setting, Setting> settingConverter,
        IBidirectionalConverter<Model.Permission, Permission> permissionConverter
    )
    {
        _settingConverter = settingConverter;
        _permissionConverter = permissionConverter;
    }

    public SettingGroup Convert(Model.SettingGroup source)
    {
        return new SettingGroup
        {
            Id = source.Id,
            CreatedBy = source.CreatedBy,
            Settings = source.Settings.Select(_settingConverter.Convert).ToList(),
            Permissions = source.Permissions.Select(_permissionConverter.Convert).ToList()
        };
    }

    public Model.SettingGroup Convert(SettingGroup source)
    {
        return new Model.SettingGroup
        {
            Id = source.Id,
            CreatedBy = source.CreatedBy,
            Settings = source.Settings.Select(_settingConverter.Convert),
            Permissions = source.Permissions.Select(_permissionConverter.Convert)
        };
    }
}
