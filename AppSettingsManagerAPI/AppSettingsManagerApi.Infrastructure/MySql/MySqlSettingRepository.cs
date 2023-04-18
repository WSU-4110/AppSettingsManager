using System.Runtime.Serialization;
using System.Text.Json;
using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Model.Requests;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class MySqlSettingRepository : ISettingRepository
{
    private readonly SettingsContext _settingsContext;
    private readonly IBidirectionalConverter<
        Model.SettingGroup,
        SettingGroup
    > _settingGroupConverter;

    public MySqlSettingRepository(
        SettingsContext settingsContext,
        IBidirectionalConverter<Model.SettingGroup, SettingGroup> settingGroupConverter
    )
    {
        // Injects SettingsContext configured in ServiceConfiguration.cs into _settingsContext object
        _settingsContext = settingsContext;
        _settingGroupConverter = settingGroupConverter;
    }

    #region Get

    public async Task<Model.SettingGroup> GetSettingGroup(string settingGroupId)
    {
        var settingGroup = await GetSettingGroupFromContext(settingGroupId);

        return _settingGroupConverter.Convert(settingGroup);
    }

    public async Task<IEnumerable<Model.SettingGroup>> GetSettingGroupsByUser(string userId)
    {
        var settingGroups = await _settingsContext.SettingGroups
            .Include(sg => sg.Settings)
            .Include(sg => sg.Permissions)
            .Where(
                sg => sg.Permissions.Any(p => p.UserId == userId && p.CurrentPermissionLevel > 0)
            )
            .ToListAsync();

        return settingGroups.Select(_settingGroupConverter.Convert);
    }

    public async Task<Dictionary<string, string>> GetSettings(GetSettingGroupRequest request)
    {
        var settings = await _settingsContext.Settings
            .Where(s => s.SettingGroupId == request.SettingGroupId && s.IsCurrent)
            .Select(s => s.Input)
            .SingleAsync();

        return JsonSerializer.Deserialize<Dictionary<string, string>>(settings)
            ?? throw new SerializationException();
    }

    #endregion

    public async Task<Model.SettingGroup> CreateSetting(CreateSettingRequest request)
    {
        var settingGroup = await GetSettingGroupFromContext(request.SettingGroupId);
        var initialSetting = !settingGroup.Settings.Any();

        var newVersionNumber = !initialSetting
            ? settingGroup.Settings.Select(s => s.Version).Max() + 1
            : 1;

        var setting = new Setting
        {
            SettingGroupId = request.SettingGroupId,
            Input = JsonSerializer.Serialize(request.Input),
            Version = newVersionNumber,
            IsCurrent = initialSetting,
            CreatedBy = request.UserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _settingsContext.Settings.Add(setting);

        await _settingsContext.SaveChangesAsync();

        return _settingGroupConverter.Convert(
            await GetSettingGroupFromContext(request.SettingGroupId)
        );
    }

    public async Task<Model.SettingGroup> CreateSettingGroup(
        string settingGroupId,
        string createdBy
    )
    {
        var settingGroup = new SettingGroup { Id = settingGroupId, CreatedBy = createdBy };

        _settingsContext.Add(settingGroup);
        await _settingsContext.SaveChangesAsync();

        return _settingGroupConverter.Convert(settingGroup);
    }

    public async Task<Model.SettingGroup> ChangeTargetSettingVersion(
        UpdateTargetSettingRequest request
    )
    {
        var settingGroup = await GetSettingGroupFromContext(request.SettingGroupId);

        var currentSetting = settingGroup.Settings.Single(s => s.IsCurrent);
        var newCurrentSetting = settingGroup.Settings.Single(
            s => s.Version == request.NewCurrentVersion
        );

        currentSetting.IsCurrent = false;
        newCurrentSetting.IsCurrent = true;

        await _settingsContext.SaveChangesAsync();

        return _settingGroupConverter.Convert(settingGroup);
    }

    public async Task<Model.SettingGroup> DeleteSetting(string settingGroupId)
    {
        var settingGroup = await GetSettingGroupFromContext(settingGroupId);

        _settingsContext.SettingGroups.Remove(settingGroup);

        await _settingsContext.SaveChangesAsync();

        return _settingGroupConverter.Convert(settingGroup);
    }

    private async Task<SettingGroup> GetSettingGroupFromContext(string settingGroupId) =>
        await _settingsContext.SettingGroups
            .Include(sg => sg.Settings)
            .Include(sg => sg.Permissions)
            .SingleAsync(sg => sg.Id == settingGroupId);
}
