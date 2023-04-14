using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Facades;

namespace AppSettingsManagerApi.Tests;

public class SettingFacadeTests
{
    private readonly Mock<ISettingRepository> _settingRepositoryMock = new();
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock = new();
    
    private readonly SettingFacade _settingFacade;

    public SettingFacadeTests()
    {
        _settingFacade = new SettingFacade(
            _settingRepositoryMock.Object,
            _permissionRepositoryMock.Object
        );
    }
    
    [Fact]
    public void Test1()
    {
    }
}