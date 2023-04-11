using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Infrastructure.MySql;
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
    
    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_ReturnsSettingGroup(
        string settingGroupId
    )
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        var expectedSettingGroup = _fixture.Build<Model.SettingGroup>().Create();
        
        _settingsContext.SettingGroups.Add(settingGroup);
        await _settingsContext.SaveChangesAsync();
        
        _settingGroupConverterMock
            .Setup(s => s.Convert(settingGroup))
            .Returns(expectedSettingGroup);
        
        var actualSettingGroup = await _settingRepository.GetSettingGroup(settingGroupId);
        
        Assert.Equal(expectedSettingGroup, actualSettingGroup);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        _settingsContext.Dispose();
    }
}