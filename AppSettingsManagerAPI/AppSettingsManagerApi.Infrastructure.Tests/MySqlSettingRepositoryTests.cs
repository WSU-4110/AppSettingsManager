using System.Runtime.Serialization.Formatters.Binary;
using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Infrastructure.MySql;
using AppSettingsManagerApi.Tests;
using AutoFixture;

namespace AppSettingsManagerApi.Infrastructure.Tests;

public class MySqlSettingRepositoryTests : IDisposable
{
    private readonly BinaryFormatter _formatter = new();
    private readonly SettingsContext _settingsContext;
    private readonly Fixture _fixture;
    private readonly Mock<
        IBidirectionalConverter<Model.SettingGroup, SettingGroup>
    > _settingGroupConverterMock = new();

    private readonly MySqlSettingRepository _settingRepository;

    public MySqlSettingRepositoryTests()
    {
        _settingsContext = SettingContextBuilder.BuildTestSettingsContext();

        _fixture = FixtureBuilder.BuildFixture();
        _settingRepository = new MySqlSettingRepository(
            _settingsContext,
            _settingGroupConverterMock.Object
        );
    }

    

    private void SetLastUpdatedAt(SettingGroup settingGroup)
    {
        using var stream = new MemoryStream();
        _formatter.Serialize(stream, DateTime.UtcNow);
        settingGroup.LastUpdatedAt = stream.ToArray();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _settingsContext.Dispose();
    }
}
