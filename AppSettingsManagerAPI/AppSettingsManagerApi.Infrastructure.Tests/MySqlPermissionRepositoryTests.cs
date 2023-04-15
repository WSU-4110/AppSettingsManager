using AutoFixture;
using AppSettingsManagerApi.Tests;
using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Model.Requests;
using AppSettingsManagerApi.Infrastructure.MySql;
using User = AppSettingsManagerApi.Infrastructure.MySql.User;
using Permission = AppSettingsManagerApi.Infrastructure.MySql.Permission;
using SettingGroup = AppSettingsManagerApi.Infrastructure.MySql.SettingGroup;
using PermissionLevel = AppSettingsManagerApi.Model.PermissionLevel;

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
    public async Task UpdatePermission_ReturnsPermission(UpdatePermissionRequest request)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = request.UserId;
        _settingsContext.Users.Add(user);

        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = request.SettingGroupId;
        _settingsContext.SettingGroups.Add(settingGroup);

        var permission = _fixture.Build<Permission>().Create();
        permission.UserId = request.UserId;
        permission.User = user;
        permission.SettingGroupId = request.SettingGroupId;
        permission.SettingGroup = settingGroup;

        _settingsContext.Permissions.Add(permission);
        await _settingsContext.SaveChangesAsync();

        var expectedPermission = _fixture.Build<Model.Permission>().Create();

        _permissionConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<Permission>(
                            p =>
                                p.SettingGroupId == request.SettingGroupId
                                && p.UserId == request.UserId
                        )
                    )
            )
            .Returns(expectedPermission);

        var actualPermission = await _permissionRepository.UpdatePermission(request);

        Assert.Equal(expectedPermission, actualPermission);
    }


    [Theory]
    [AutoTestData]
    public async Task GetPermission_ThrowsInvalidOperationException(string userId, string settingGroupId)
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _permissionRepository.GetPermission(userId, settingGroupId)
        );

    }

    [Theory]
    [AutoTestData]
    public async Task GetPermission_ReturnsPermission(string userId, string settingGroupId)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = userId;
        _settingsContext.Users.Add(user);

        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;
        _settingsContext.SettingGroups.Add(settingGroup);

        var permission = _fixture.Build<Permission>().Create();
        permission.UserId = userId;
        permission.User = user;
        permission.SettingGroupId = settingGroupId;
        permission.SettingGroup = settingGroup;

        _settingsContext.Permissions.Add(permission);
        await _settingsContext.SaveChangesAsync();

        var expectedPermission = _fixture.Build<Model.Permission>().Create();

        _permissionConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<Permission>(
                            p => p.SettingGroupId == settingGroupId && p.UserId == userId
                        )
                    )
            )
            .Returns(expectedPermission);

        var actualPermission = await _permissionRepository.GetPermission(userId, settingGroupId);

        Assert.Equal(expectedPermission, actualPermission);
    }

    [Theory]
    [AutoTestData]
    public async Task DeletePermission_ReturnsPermission(string userId, string settingGroupId)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = userId;
        _settingsContext.Users.Add(user);

        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;
        _settingsContext.SettingGroups.Add(settingGroup);

        var permission = _fixture.Build<Permission>().Create();
        permission.UserId = userId;
        permission.User = user;
        permission.SettingGroupId = settingGroupId;
        permission.SettingGroup = settingGroup;

        _settingsContext.Permissions.Add(permission);
        await _settingsContext.SaveChangesAsync();

        var expectedPermission = _fixture.Build<Model.Permission>().Create();

        _permissionConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<Permission>(
                            p => p.SettingGroupId == settingGroupId && p.UserId == userId
                        )
                    )
            )
            .Returns(expectedPermission);

        var actualPermission = await _permissionRepository.DeletePermission(userId, settingGroupId);

        Assert.Equal(expectedPermission, actualPermission);
    }

    [Theory]
    [AutoTestData]
    public async Task CreatePermission_ReturnsPermission(CreatePermissionRequest request)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = request.UserId;
        _settingsContext.Users.Add(user);

        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = request.SettingGroupId;
        _settingsContext.SettingGroups.Add(settingGroup);

        var permission = _fixture.Build<Permission>().Create();
        permission.UserId = request.UserId;
        permission.User = user;
        permission.SettingGroupId = request.SettingGroupId;
        permission.SettingGroup = settingGroup;

        var expectedPermission = _fixture.Build<Model.Permission>().Create();

        _permissionConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<Permission>(
                            p =>
                                p.SettingGroupId == request.SettingGroupId
                                && p.UserId == request.UserId
                        )
                    )
            )
            .Returns(expectedPermission);

        var actualPermission = await _permissionRepository.CreatePermission(request);

        Assert.Equal(expectedPermission, actualPermission);
    }

    [Theory]
    [AutoTestData]
    public async Task CreatePermission_TrowsInvalidOperationException(CreatePermissionRequest request)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = request.UserId;
        _settingsContext.Users.Add(user);

        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = request.SettingGroupId;
        _settingsContext.SettingGroups.Add(settingGroup);

        var permission = _fixture.Build<Permission>().Create();
        permission.UserId = request.UserId;
        permission.User = user;
        permission.SettingGroupId = request.SettingGroupId;
        permission.SettingGroup = settingGroup;

        _settingsContext.Permissions.Add(permission);
        await _settingsContext.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _permissionRepository.CreatePermission(request)
        );
    }

    [Theory]
    [AutoTestData]
    public async Task GetPermissionFromContext_ReturnsPermission(string userId, string settingGroupId)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = userId;
        _settingsContext.Users.Add(user);

        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;
        _settingsContext.SettingGroups.Add(settingGroup);

        var permission = _fixture.Build<Permission>().Create();
        permission.UserId = userId;
        permission.User = user;
        permission.SettingGroupId = settingGroupId;
        permission.SettingGroup = settingGroup;

        _settingsContext.Permissions.Add(permission);
        await _settingsContext.SaveChangesAsync();

        var actualPermission = await _permissionRepository.GetPermissionFromContext(userId, settingGroupId);

        Assert.Equal(permission, actualPermission);
    }

    [Theory]
    [AutoTestData]
    public async Task CheckPermission_ThrowsInvalidOperationException(string userId, string password, string settingGroupId, PermissionLevel permissionLevel)
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _permissionRepository.CheckPermission(userId, password, settingGroupId, permissionLevel)
        );
    }
    

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _settingsContext.Dispose();
    }
}