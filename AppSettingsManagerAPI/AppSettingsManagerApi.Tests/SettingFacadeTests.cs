using System.Runtime.CompilerServices;
using System.Text.Json;
using AppSettingsManagerApi.Domain.Exceptions;
using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Facades;
using AppSettingsManagerApi.Model;
using AppSettingsManagerApi.Model.Requests;
using AutoFixture;
using Microsoft.AspNetCore.Server.HttpSys;

namespace AppSettingsManagerApi.Tests;

public class SettingFacadeTests
{
    private readonly Mock<ISettingRepository> _settingRepositoryMock = new();
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock = new();

    private readonly Fixture _fixture;

    private readonly SettingFacade _settingFacade;

    public SettingFacadeTests()
    {
        _settingFacade = new SettingFacade(
            _settingRepositoryMock.Object,
            _permissionRepositoryMock.Object
        );

        _fixture = FixtureBuilder.BuildFixture();
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_Succeeds(GetSettingGroupRequest request)
    {
        SetupCheckPermissions_Success(request.UserId, request.Password, request.SettingGroupId);

        var convertedSettingGroup = _fixture.Build<SettingGroup>().Create();

        _settingRepositoryMock
            .Setup(x => x.GetSettingGroup(It.Is<string>(id => id == request.SettingGroupId)))
            .ReturnsAsync(convertedSettingGroup);

        var settingGroup = await _settingFacade.GetSettingGroup(request);

        Assert.Equal(convertedSettingGroup, settingGroup);
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_IncorrectPassword(GetSettingGroupRequest request)
    {
        SetupCheckPermissions_IncorrectPassword(
            request.UserId,
            request.Password,
            request.SettingGroupId
        );

        var convertedSettingGroup = _fixture.Build<SettingGroup>().Create();

        _settingRepositoryMock
            .Setup(x => x.GetSettingGroup(It.Is<string>(id => id == request.SettingGroupId)))
            .ReturnsAsync(convertedSettingGroup);

        await Assert.ThrowsAsync<IncorrectPasswordException>(
            () => _settingFacade.GetSettingGroup(request)
        );
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_InsufficientPermissions(GetSettingGroupRequest request)
    {
        SetupCheckPermissions_InsufficientPermissions(
            request.UserId,
            request.Password,
            request.SettingGroupId
        );

        var convertedSettingGroup = _fixture.Build<SettingGroup>().Create();

        _settingRepositoryMock
            .Setup(x => x.GetSettingGroup(It.Is<string>(id => id == request.SettingGroupId)))
            .ReturnsAsync(convertedSettingGroup);

        await Assert.ThrowsAsync<InsufficientPermissionException>(
            () => _settingFacade.GetSettingGroup(request)
        );
    }

    [Theory]
    [AutoTestData]
    public async Task GetAllSettingGroupsForUser_Succeeds(GetSettingGroupRequest request)
    {
        SetupCheckPermissions_Success(request.UserId, request.Password, request.SettingGroupId);

        var convertedSettingGroups = _fixture.Build<SettingGroup>().CreateMany(10).ToList();

        _settingRepositoryMock
            .Setup(x => x.GetSettingGroupsByUser(It.Is<string>(id => id == request.UserId)))
            .ReturnsAsync(convertedSettingGroups);

        var settingGroups = await _settingFacade.GetAllSettingGroupsForUser(request);

        Assert.True(
            convertedSettingGroups
                .OrderBy(sg => sg.Id)
                .SequenceEqual(settingGroups.OrderBy(sg => sg.Id))
        );
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettingsSucceeds(
        GetSettingGroupRequest request,
        string testKey,
        string testValue
    )
    {
        SetupCheckPermissions_Success(request.UserId, request.Password, request.SettingGroupId);

        var settings = new Dictionary<string, string> { { testKey, testValue } };

        _settingRepositoryMock
            .Setup(
                x =>
                    x.GetSettings(
                        It.Is<GetSettingGroupRequest>(
                            req => req.SettingGroupId == request.SettingGroupId
                        )
                    )
            )
            .ReturnsAsync(settings);

        var result = await _settingFacade.GetSettings(request);

        Assert.Equal(settings, result);
    }

    [Theory]
    [AutoTestData]
    public async Task GetSetting_Succeeds(
        GetSettingGroupRequest request,
        string testKey,
        string testValue
    )
    {
        SetupCheckPermissions_Success(request.UserId, request.Password, request.SettingGroupId);

        var settings = new Dictionary<string, string> { { testKey, testValue } };

        _settingRepositoryMock
            .Setup(
                x =>
                    x.GetSettings(
                        It.Is<GetSettingGroupRequest>(
                            req => req.SettingGroupId == request.SettingGroupId
                        )
                    )
            )
            .ReturnsAsync(settings);

        var result = await _settingFacade.GetSetting(request, testKey);

        Assert.Equal(testValue, result);
    }

    [Theory]
    [AutoTestData]
    public async Task CreateSettingGroup_Succeeds(Dictionary<string, string> input)
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        var setting = _fixture.Build<Setting>().Create();
        setting.SettingGroupId = settingGroup.Id;
        settingGroup.Settings = new List<Setting> { setting };

        var user = _fixture.Build<User>().Create();
        var permission = _fixture.Build<Permission>().Create();
        permission.SettingGroupId = settingGroup.Id;
        permission.UserId = user.UserId;
        settingGroup.Permissions = new List<Permission> { permission };

        var createSettingRequest = new CreateSettingRequest
        {
            SettingGroupId = settingGroup.Id,
            Input = JsonSerializer.SerializeToNode(input),
            UserId = user.UserId,
            Password = user.Password
        };

        _settingRepositoryMock
            .Setup(
                s =>
                    s.CreateSettingGroup(
                        It.Is<string>(sgId => sgId == settingGroup.Id),
                        It.Is<string>(uId => uId == user.UserId)
                    )
            )
            .ReturnsAsync(settingGroup);

        _settingRepositoryMock
            .Setup(
                s =>
                    s.CreateSetting(
                        It.Is<CreateSettingRequest>(req => req.SettingGroupId == settingGroup.Id)
                    )
            )
            .ReturnsAsync(settingGroup);

        _permissionRepositoryMock
            .Setup(
                p =>
                    p.CreatePermission(
                        It.Is<CreatePermissionRequest>(
                            req =>
                                req.SettingGroupId == settingGroup.Id && req.UserId == user.UserId
                        )
                    )
            )
            .ReturnsAsync(permission);

        _settingRepositoryMock
            .Setup(s => s.GetSettingGroup(It.Is<string>(sgId => sgId == settingGroup.Id)))
            .ReturnsAsync(settingGroup);

        var result = await _settingFacade.CreateSettingGroup(createSettingRequest);

        Assert.Equal(settingGroup, result);
    }

    [Theory]
    [AutoTestData]
    public async Task UpdateSetting_Succeeds(
        Dictionary<string, string> input,
        string userId,
        string password,
        string settingGroupId
    )
    {
        SetupCheckPermissions_Success(userId, password, settingGroupId);

        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;

        var setting = _fixture.Build<Setting>().CreateMany(10);

        var createSettingRequest = new CreateSettingRequest
        {
            SettingGroupId = settingGroupId,
            Input = JsonSerializer.SerializeToNode(input),
            UserId = userId,
            Password = password
        };

        _settingRepositoryMock
            .Setup(
                s =>
                    s.CreateSetting(
                        It.Is<CreateSettingRequest>(req => req.SettingGroupId == settingGroupId)
                    )
            )
            .ReturnsAsync(settingGroup);

        var result = await _settingFacade.UpdateSetting(createSettingRequest);

        Assert.Equal(settingGroup, result);
    }

    [Theory]
    [AutoTestData]
    public async Task ChangeTargetSettingVersion_Success(UpdateTargetSettingRequest request)
    {
        SetupCheckPermissions_Success(request.UserId, request.Password, request.SettingGroupId);

        var settingGroup = _fixture.Build<SettingGroup>().Create();

        _settingRepositoryMock
            .Setup(
                s =>
                    s.ChangeTargetSettingVersion(
                        It.Is<UpdateTargetSettingRequest>(
                            req => req.SettingGroupId == request.SettingGroupId
                        )
                    )
            )
            .ReturnsAsync(settingGroup);

        var result = await _settingFacade.ChangeTargetSettingVersion(request);

        Assert.Equal(settingGroup, result);
    }

    [Theory]
    [AutoTestData]
    public async Task DeleteSettingGroup_Succeeds(DeleteSettingGroupRequest request)
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();

        SetupCheckPermissions_Success(request.UserId, request.Password, request.SettingGroupId);

        _settingRepositoryMock
            .Setup(s => s.DeleteSettingGroup(It.Is<string>(sgId => sgId == request.SettingGroupId)))
            .ReturnsAsync(settingGroup);

        var result = await _settingFacade.DeleteSettingGroup(request);

        Assert.Equal(settingGroup, result);
    }

    [Theory]
    [AutoTestData]
    public async Task RequestNewSettingGroupAccess_Succeeds(CreatePermissionRequest request)
    {
        var permission = _fixture.Build<Permission>().Create();

        _permissionRepositoryMock
            .Setup(
                p =>
                    p.CreatePermission(
                        It.Is<CreatePermissionRequest>(
                            request =>
                                request.SettingGroupId == request.SettingGroupId
                                && request.UserId == request.UserId
                        )
                    )
            )
            .ReturnsAsync(permission);

        var result = await _settingFacade.RequestNewSettingGroupAccess(request);

        Assert.Equal(permission, result);
    }

    [Theory]
    [AutoTestData]
    public async Task UpdateSettingGroupAccess(UpdatePermissionRequest request)
    {
        var permission = _fixture.Build<Permission>().Create();
        
        _permissionRepositoryMock
            .Setup(
                p =>
                    p.UpdatePermission(
                        It.Is<UpdatePermissionRequest>(
                            req =>
                                req.SettingGroupId == request.SettingGroupId
                                && req.UserId == request.UserId
                        )
                    )
            )
            .ReturnsAsync(permission);
        
        var result = await _settingFacade.UpdateSettingGroupAccess(request);
        
        Assert.Equal(permission, result);
    }

    [Theory]
    [AutoTestData]
    public async Task PermissionRequestResponse_Success(PermissionRequestResponse response)
    {
        SetupCheckPermissions_Success(response.ApproverId, response.Password, response.SettingGroupId);
        
        var permission = _fixture.Build<Permission>().Create();
        
        _permissionRepositoryMock
            .Setup(
                p =>
                    p.PermissionRequestResponse(
                        It.Is<PermissionRequestResponse>(
                            req =>
                                req.SettingGroupId == response.SettingGroupId
                                && req.UserId == response.UserId
                                && req.ApproverId == response.ApproverId
                        )
                    )
            )
            .ReturnsAsync(permission);
        
        var result = await _settingFacade.PermissionRequestResponse(response);
        
        Assert.Equal(permission, result);
    }

    private void SetupCheckPermissions_Success(
        string userId,
        string password,
        string settingGroupId
    )
    {
        _permissionRepositoryMock
            .Setup(
                x =>
                    x.CheckPermission(
                        It.Is<string>(id => id == userId),
                        It.Is<string>(pwd => pwd == password),
                        It.Is<string>(id => id == settingGroupId),
                        It.IsAny<PermissionLevel>()
                    )
            )
            .Returns(Task.CompletedTask);
    }

    private void SetupCheckPermissions_IncorrectPassword(
        string userId,
        string password,
        string settingGroupId
    )
    {
        _permissionRepositoryMock
            .Setup(
                x =>
                    x.CheckPermission(
                        It.Is<string>(id => id == userId),
                        It.Is<string>(pwd => pwd == password),
                        It.Is<string>(id => id == settingGroupId),
                        It.IsAny<PermissionLevel>()
                    )
            )
            .ThrowsAsync(new IncorrectPasswordException(userId));
    }

    private void SetupCheckPermissions_InsufficientPermissions(
        string userId,
        string password,
        string settingGroupId
    )
    {
        _permissionRepositoryMock
            .Setup(
                x =>
                    x.CheckPermission(
                        It.Is<string>(id => id == userId),
                        It.Is<string>(pwd => pwd == password),
                        It.Is<string>(id => id == settingGroupId),
                        It.IsAny<PermissionLevel>()
                    )
            )
            .ThrowsAsync(new InsufficientPermissionException(userId, settingGroupId));
    }
}
