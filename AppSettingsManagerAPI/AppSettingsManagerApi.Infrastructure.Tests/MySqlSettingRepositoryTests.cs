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

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_Succeeds(string settingGroupId)
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;
        
        _settingsContext.SettingGroups.Add(settingGroup);
        
        var convertedSettingGroup = _fixture.Build<Model.SettingGroup>().Create();
        _settingGroupConverterMock.Setup(c => c.Convert(It.Is<SettingGroup>(s => s.Id == settingGroupId))).Returns(convertedSettingGroup);
        
        var result = await _settingRepository.GetSettingGroup(settingGroupId);
        
        Assert.Equal(convertedSettingGroup, result);
    }
       
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        _settingsContext.Dispose();
    }
}