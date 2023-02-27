using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class SettingGroupBiverter : IBidirectionalConverter<Model.SettingGroup, SettingGroup>
{
    private readonly IBidirectionalConverter<
        Model.Setting,
        Setting
    > _settingVersionBiverter;

    public SettingGroupBiverter(
        IBidirectionalConverter<Model.Setting, Setting> settingVersionBiverter
    )
    {
        _settingVersionBiverter = settingVersionBiverter;
    }

    public SettingGroup Convert(Model.SettingGroup source)
    {
        return new SettingGroup
        {
            SettingId = source.SettingId,
            CreatedBy = source.CreatedBy,
            Settings = source.Settings
                .Select(_settingVersionBiverter.Convert)
                .ToList()
        };
    }

    public Model.SettingGroup Convert(SettingGroup source)
    {
        return new Model.SettingGroup
        {
            SettingId = source.SettingId,
            CreatedBy = source.CreatedBy,
            Settings = source.Settings.Select(_settingVersionBiverter.Convert)
        };
    }
}
