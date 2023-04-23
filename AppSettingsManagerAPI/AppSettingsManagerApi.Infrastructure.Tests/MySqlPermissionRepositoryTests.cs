using AutoFixture;
using AppSettingsManagerApi.Tests;
using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Model.Requests;
using AppSettingsManagerApi.Infrastructure.MySql;
using Microsoft.EntityFrameworkCore;
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
    public async Task GetPermission_ThrowsInvalidOperationException(
        string userId,
        string settingGroupId
    )
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
    public async Task CreatePermission_TrowsInvalidOperationException(
        CreatePermissionRequest request
    )
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
    public async Task CheckPermission_ThrowsInvalidOperationException(
        string userId,
        string password,
        string settingGroupId,
        PermissionLevel permissionLevel
    )
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () =>
                _permissionRepository.CheckPermission(
                    userId,
                    password,
                    settingGroupId,
                    permissionLevel
                )
        );
    }

    [Theory]
    [AutoTestData]
    public async Task PermissionRequestResponse_Approved(PermissionRequestResponse response)
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();

        var userAdmin = _fixture.Build<User>().Create();
        var admin = new Permission
        {
            SettingGroupId = settingGroup.Id,
            UserId = userAdmin.Id,
            CurrentPermissionLevel = PermissionLevel.Admin,
            SettingGroup = settingGroup,
            User = userAdmin,
            ApprovedBy = string.Empty,
            RequestedPermissionLevel = PermissionLevel.None,
            WaitingForApproval = false
        };

        var newUser = _fixture.Build<User>().Create();
        var newPermission = new Permission
        {
            SettingGroupId = settingGroup.Id,
            UserId = newUser.Id,
            CurrentPermissionLevel = PermissionLevel.None,
            SettingGroup = settingGroup,
            User = newUser,
            ApprovedBy = string.Empty,
            RequestedPermissionLevel = PermissionLevel.Admin,
            WaitingForApproval = true
        };

        _settingsContext.SettingGroups.Add(settingGroup);
        _settingsContext.Users.Add(userAdmin);
        _settingsContext.Permissions.Add(admin);
        _settingsContext.Users.Add(newUser);
        _settingsContext.Permissions.Add(newPermission);

        await _settingsContext.SaveChangesAsync();

        var convertedPermission = _fixture.Build<Model.Permission>().Create();

        _permissionConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<Permission>(
                            p =>
                                p.SettingGroupId == settingGroup.Id
                                && p.UserId == newUser.Id
                                && p.CurrentPermissionLevel == PermissionLevel.Admin
                                && p.RequestedPermissionLevel == PermissionLevel.None
                                && p.WaitingForApproval == false
                                && p.ApprovedBy == userAdmin.Id
                        )
                    )
            )
            .Returns(convertedPermission);

        response.Approved = true;
        response.SettingGroupId = settingGroup.Id;
        response.ApproverId = userAdmin.Id;
        response.UserId = newUser.Id;
        response.Password = userAdmin.Password;

        var actualPermission = await _permissionRepository.PermissionRequestResponse(response);

        Assert.Equal(convertedPermission, actualPermission);
    }

    [Theory]
    [AutoTestData]
    public async Task PermissionRequestResponse_Denied(PermissionRequestResponse response)
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();

        var userAdmin = _fixture.Build<User>().Create();
        var admin = new Permission
        {
            SettingGroupId = settingGroup.Id,
            UserId = userAdmin.Id,
            CurrentPermissionLevel = PermissionLevel.Admin,
            SettingGroup = settingGroup,
            User = userAdmin,
            ApprovedBy = string.Empty,
            RequestedPermissionLevel = PermissionLevel.None,
            WaitingForApproval = false
        };

        var newUser = _fixture.Build<User>().Create();
        var newPermission = new Permission
        {
            SettingGroupId = settingGroup.Id,
            UserId = newUser.Id,
            CurrentPermissionLevel = PermissionLevel.None,
            SettingGroup = settingGroup,
            User = newUser,
            ApprovedBy = string.Empty,
            RequestedPermissionLevel = PermissionLevel.Admin,
            WaitingForApproval = true
        };

        _settingsContext.SettingGroups.Add(settingGroup);
        _settingsContext.Users.Add(userAdmin);
        _settingsContext.Permissions.Add(admin);
        _settingsContext.Users.Add(newUser);
        _settingsContext.Permissions.Add(newPermission);

        await _settingsContext.SaveChangesAsync();

        var convertedPermission = _fixture.Build<Model.Permission>().Create();

        _permissionConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<Permission>(
                            p =>
                                p.SettingGroupId == settingGroup.Id
                                && p.UserId == newUser.Id
                                && p.CurrentPermissionLevel == PermissionLevel.None
                                && p.RequestedPermissionLevel == PermissionLevel.None
                                && p.WaitingForApproval == false
                                && p.ApprovedBy == string.Empty
                        )
                    )
            )
            .Returns(convertedPermission);

        response.Approved = false;
        response.SettingGroupId = settingGroup.Id;
        response.ApproverId = userAdmin.Id;
        response.UserId = newUser.Id;
        response.Password = userAdmin.Password;

        var actualPermission = await _permissionRepository.PermissionRequestResponse(response);

        Assert.Equal(convertedPermission, actualPermission);
    }

    [Theory]
    [AutoTestData]
    public async Task DeletePermission_Succeeds(string userId, string settingGroupId)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = userId;
        user.Permissions = new List<Permission>();
        _settingsContext.Users.Add(user);

        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;
        settingGroup.Permissions = new List<Permission>();
        _settingsContext.SettingGroups.Add(settingGroup);

        var permission = _fixture.Build<Permission>().Create();
        permission.UserId = userId;
        permission.User = user;
        permission.SettingGroupId = settingGroupId;
        permission.SettingGroup = settingGroup;

        _settingsContext.Permissions.Add(permission);
        await _settingsContext.SaveChangesAsync();

        var convertedPermission = _fixture.Build<Model.Permission>().Create();

        _permissionConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<Permission>(
                            p => p.SettingGroupId == settingGroup.Id && p.UserId == user.Id
                        )
                    )
            )
            .Returns(convertedPermission);

        var actualPermission = await _permissionRepository.DeletePermission(userId, settingGroupId);

        Assert.Equal(convertedPermission, actualPermission);
        await Assert.ThrowsAsync<InvalidOperationException>(
            () =>
                _settingsContext.Permissions.SingleAsync(
                    p => p.SettingGroupId == settingGroupId && p.UserId == userId
                )
        );
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _settingsContext.Dispose();
    }
}
