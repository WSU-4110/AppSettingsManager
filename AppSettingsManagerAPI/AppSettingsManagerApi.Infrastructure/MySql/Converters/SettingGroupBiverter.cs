using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class SettingGroupBiverter : IBidirectionalConverter<Model.SettingGroup, SettingGroup>
{
    private readonly IBidirectionalConverter<
        Model.SettingVersion,
        SettingVersion
    > _settingVersionBiverter;

    public SettingGroupBiverter(
        IBidirectionalConverter<Model.SettingVersion, SettingVersion> settingVersionBiverter
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
            SettingVersions = source.SettingVersions
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
            SettingVersions = source.SettingVersions.Select(_settingVersionBiverter.Convert)
        };
    }
}
