using System.Runtime.Serialization.Formatters.Binary;
using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Infrastructure.MySql;
using AppSettingsManagerApi.Model;
using AppSettingsManagerApi.Model.Requests;
using AppSettingsManagerApi.Tests;
using AutoFixture;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Permission = AppSettingsManagerApi.Infrastructure.MySql.Permission;
using Setting = AppSettingsManagerApi.Infrastructure.MySql.Setting;
using SettingGroup = AppSettingsManagerApi.Infrastructure.MySql.SettingGroup;
using User = AppSettingsManagerApi.Infrastructure.MySql.User;

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

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_ReturnsSettingGroup(string settingGroupId)
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        SetLastUpdatedAt_SettingGroup(settingGroup);
        settingGroup.Id = settingGroupId;
        var expectedSettingGroup = _fixture.Build<Model.SettingGroup>().Create();

        _settingsContext.SettingGroups.Add(settingGroup);
        await _settingsContext.SaveChangesAsync();

        _settingGroupConverterMock
            .Setup(s => s.Convert(settingGroup))
            .Returns(expectedSettingGroup);

        var actualSettingGroup = await _settingRepository.GetSettingGroup(settingGroupId);

        Assert.Equal(expectedSettingGroup, actualSettingGroup);
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroup_ThrowsInvalidOperationException(string settingGroupId)
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _settingRepository.GetSettingGroup(settingGroupId)
        );
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroupsByUser_Succeeds(string userId)
    {
        var settingGroups = _fixture.Build<SettingGroup>().CreateMany(10).ToList();
        var expectedSettingGroups = _fixture.Build<Model.SettingGroup>().CreateMany(10).ToList();
        var user = new User
        {
            Id = userId,
            Password = "test",
            Email = "test"
        };

        var permissions = new List<Permission>();
        foreach (var settingGroup in settingGroups)
        {
            SetLastUpdatedAt_SettingGroup(settingGroup);

            var permission = new Permission
            {
                UserId = userId,
                User = user,
                SettingGroupId = settingGroup.Id,
                SettingGroup = settingGroup,
                ApprovedBy = "test",
                CurrentPermissionLevel = PermissionLevel.Read,
                RequestedPermissionLevel = PermissionLevel.None,
                WaitingForApproval = false
            };

            permissions.Add(permission);

            settingGroup.Permissions = new List<Permission> { permission };
        }

        _settingsContext.SettingGroups.AddRange(settingGroups);
        _settingsContext.Users.Add(user);
        _settingsContext.Permissions.AddRange(permissions);

        await _settingsContext.SaveChangesAsync();

        for (var i = 0; i < settingGroups.Count; i++)
        {
            var i1 = i;
            _settingGroupConverterMock
                .Setup(
                    s =>
                        s.Convert(
                            It.Is<SettingGroup>(
                                settingGroup => settingGroup.Id == settingGroups[i1].Id
                            )
                        )
                )
                .Returns(expectedSettingGroups[i1]);
        }

        var actualSettingGroups = (
            await _settingRepository.GetSettingGroupsByUser(user.Id)
        ).ToList();

        Assert.True(
            expectedSettingGroups
                .OrderBy(s => s.Id)
                .SequenceEqual(actualSettingGroups.OrderBy(s => s.Id))
        );
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettingGroupsByUser_ReturnsEmptyList(string userId)
    {
        var actualSettingGroups = (
            await _settingRepository.GetSettingGroupsByUser(userId)
        ).ToList();

        Assert.Empty(actualSettingGroups);
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettings_Success(
        GetSettingGroupRequest request,
        Dictionary<string, string> input
    )
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = request.SettingGroupId;

        var setting = new Setting
        {
            SettingGroupId = settingGroup.Id,
            SettingGroup = settingGroup,
            CreatedAt = DateTimeOffset.Now,
            CreatedBy = "test",
            Input = JsonSerializer.Serialize(input),
            IsCurrent = true,
            Version = 1
        };
        SetLastUpdatedAt_Setting(setting);

        SetLastUpdatedAt_SettingGroup(settingGroup);
        settingGroup.Settings = new List<Setting> { setting };

        _settingsContext.SettingGroups.Add(settingGroup);
        await _settingsContext.SaveChangesAsync();

        var actualInput = await _settingRepository.GetSettings(request);

        Assert.Equal(input, actualInput);
    }

    [Theory]
    [AutoTestData]
    public async Task GetSettings_ThrowsInvalidOperationException(GetSettingGroupRequest request)
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _settingRepository.GetSettings(request)
        );
    }

    [Theory]
    [AutoTestData]
    public async Task CreateSettingGroup_Success(string settingGroupId, string createdBy)
    {
        var convertedSettingGroup = _fixture.Build<Model.SettingGroup>().Create();

        _settingGroupConverterMock
            .Setup(s => s.Convert(It.Is<SettingGroup>(sg => sg.Id == settingGroupId)))
            .Returns(convertedSettingGroup);

        var actualSettingGroup = await _settingRepository.CreateSettingGroup(
            settingGroupId,
            createdBy
        );

        Assert.Equal(convertedSettingGroup, actualSettingGroup);
    }

    [Theory]
    [AutoTestData]
    public async Task CreateSettingGroup_ThrowsInvalidOperationException(
        string settingGroupId,
        string createdBy
    )
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;

        _settingsContext.SettingGroups.Add(settingGroup);
        await _settingsContext.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _settingRepository.CreateSettingGroup(settingGroupId, createdBy)
        );
    }

    [Theory]
    [AutoTestData]
    public async Task CreateSetting_Succeeds(
        Dictionary<string, string> input,
        string settingGroupId,
        string userId,
        string password
    )
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;
        settingGroup.Settings = new List<Setting>();
        SetLastUpdatedAt_SettingGroup(settingGroup);

        var convertedSettingGroup = _fixture.Build<Model.SettingGroup>().Create();

        _settingGroupConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<SettingGroup>(
                            sg => sg.Id == settingGroupId && sg.Settings.Single().Version == 1
                        )
                    )
            )
            .Returns(convertedSettingGroup);

        _settingsContext.SettingGroups.Add(settingGroup);
        await _settingsContext.SaveChangesAsync();

        var result = await _settingRepository.CreateSetting(
            new CreateSettingRequest
            {
                SettingGroupId = settingGroupId,
                Input = JsonSerializer.SerializeToNode(input),
                UserId = userId,
                Password = password
            }
        );

        Assert.Equal(convertedSettingGroup, result);
    }

    [Theory]
    [AutoTestData]
    public async Task CreateSetting_ThrowsInvalidOperationException(
        Dictionary<string, string> input,
        string settingGroupId,
        string userId,
        string password
    )
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () =>
                _settingRepository.CreateSetting(
                    new CreateSettingRequest
                    {
                        SettingGroupId = settingGroupId,
                        Input = JsonSerializer.SerializeToNode(input),
                        UserId = userId,
                        Password = password
                    }
                )
        );
    }

    [Theory]
    [AutoTestData]
    public async Task ChangeTargetSettingVersion_Succeeds(UpdateTargetSettingRequest request)
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = request.SettingGroupId;
        SetLastUpdatedAt_SettingGroup(settingGroup);

        var settings = _fixture.Build<Setting>().CreateMany(10).ToList();
        for (var i = 0; i < settings.Count; i++)
        {
            settings.ElementAt(i).SettingGroupId = settingGroup.Id;
            settings.ElementAt(i).SettingGroup = settingGroup;
            settings.ElementAt(i).Version = i + 1;
            SetLastUpdatedAt_Setting(settings.ElementAt(i));
            settings.ElementAt(i).IsCurrent = i == 0;
        }

        settingGroup.Settings = settings.ToList();

        var user = _fixture.Build<User>().Create();
        var permission = _fixture.Build<Permission>().Create();

        permission.SettingGroupId = settingGroup.Id;
        permission.UserId = user.Id;
        permission.CurrentPermissionLevel = PermissionLevel.Admin;

        _settingsContext.SettingGroups.Add(settingGroup);
        _settingsContext.Settings.AddRange(settings);
        _settingsContext.Users.Add(user);
        _settingsContext.Permissions.Add(permission);

        await _settingsContext.SaveChangesAsync();

        var convertedSettingGroup = _fixture.Build<Model.SettingGroup>().Create();
        request.UserId = user.Id;
        request.SettingGroupId = settingGroup.Id;
        request.Password = user.Password;
        request.NewCurrentVersion = 5;

        _settingGroupConverterMock
            .Setup(
                s =>
                    s.Convert(
                        It.Is<SettingGroup>(
                            sg =>
                                sg.Id == settingGroup.Id
                                && sg.Settings.Single(setting => setting.IsCurrent).Version == 5
                        )
                    )
            )
            .Returns(convertedSettingGroup);

        var result = await _settingRepository.ChangeTargetSettingVersion(request);

        Assert.Equal(convertedSettingGroup, result);
    }

    [Theory]
    [AutoTestData]
    public async Task DeleteSettingGroup_Succeeds(string settingGroupId)
    {
        var settingGroup = _fixture.Build<SettingGroup>().Create();
        settingGroup.Id = settingGroupId;
        SetLastUpdatedAt_SettingGroup(settingGroup);

        var convertedSettingGroup = _fixture.Build<Model.SettingGroup>().Create();

        _settingGroupConverterMock
            .Setup(s => s.Convert(It.Is<SettingGroup>(sg => sg.Id == settingGroupId)))
            .Returns(convertedSettingGroup);

        _settingsContext.SettingGroups.Add(settingGroup);

        await _settingsContext.SaveChangesAsync();

        var result = await _settingRepository.DeleteSettingGroup(settingGroupId);

        Assert.Equal(convertedSettingGroup, result);
        Assert.Empty(_settingsContext.SettingGroups);
    }

    private void SetLastUpdatedAt_SettingGroup(SettingGroup settingGroup)
    {
        using var stream = new MemoryStream();
        _formatter.Serialize(stream, DateTime.UtcNow);
        settingGroup.LastUpdatedAt = stream.ToArray();
    }

    private void SetLastUpdatedAt_Setting(Setting setting)
    {
        using var stream = new MemoryStream();
        _formatter.Serialize(stream, DateTime.UtcNow);
        setting.LastUpdatedAt = stream.ToArray();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _settingsContext.Dispose();
    }
}
