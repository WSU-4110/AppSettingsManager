using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class SettingBiverter : IBidirectionalConverter<Model.Setting, Setting>
{
    public Setting Convert(Model.Setting input)
    {
        throw new NotImplementedException();
    }

    public Model.Setting Convert(Setting input)
    {
        throw new NotImplementedException();
    }
}