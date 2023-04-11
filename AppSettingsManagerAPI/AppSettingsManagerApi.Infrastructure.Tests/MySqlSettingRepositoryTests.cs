using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Infrastructure.MySql;
using AppSettingsManagerApi.Model.Requests;
using AppSettingsManagerApi.Tests;
using AutoFixture;

namespace AppSettingsManagerApi.Infrastructure.Tests;

public class MySqlSettingRepositoryTests : IDisposable
{
    private readonly SettingsContext _settingsContext;
    private readonly Fixture _fixture;
    private readonly Mock<IBidirectionalConverter<Model.SettingGroup, SettingGroup>> _settingGroupConverterMock = new ();
    
    private readonly MySqlSettingRepository _settingRepository;
    
    public MySqlSettingRepositoryTests()
    {
        _settingsContext = SettingContextBuilder.BuildTestSettingsContext();
        _fixture = FixtureBuilder.BuildFixture();
        _settingRepository = new MySqlSettingRepository(_settingsContext, _settingGroupConverterMock.Object);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        _settingsContext.Dispose();
    }
}