using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Model;
using AppSettingsManagerApi.Infrastructure.MySql;
using Permission = AppSettingsManagerApi.Infrastructure.MySql.Permission;
using AppSettingsManagerApi.Tests;
using AutoFixture;
namespace AppSettingsManagerApi.Infrastructure.Tests;

public class MySqlPermissionRepositoryTests : IDisposable
{
    private readonly SettingsContext _settingsContext;
    private readonly Fixture _fixture;
    private readonly Mock<
        IBidirectionalConverter<Model.Permission, Permission>
    > _permissionConverterMock = new();

    private readonly MySqlPermissionRepository _permissionRepository;

    public MySqlPermissionRepositoryTests()
    {
        _settingsContext = SettingContextBuilder.BuildTestSettingsContext();

        _fixture = FixtureBuilder.BuildFixture();
        _permissionRepository = new MySqlPermissionRepository(
            _settingsContext,
            _permissionConverterMock.Object
        );
    }
    
    [Theory]
    [AutoTestData]
    public async Task GetPermission_ThrowsInvalidOperationException(string userId, string settingGroupId)
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _permissionRepository.GetPermission(userId, settingGroupId)
        );

    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _settingsContext.Dispose();
    }
}