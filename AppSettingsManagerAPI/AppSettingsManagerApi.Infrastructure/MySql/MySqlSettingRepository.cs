using System.Text.Json;
using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Model.Requests;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class MySqlSettingRepository : ISettingRepository
{
    // Creates SettingsContext object
    private readonly SettingsContext _settingsContext;
    private readonly IBidirectionalConverter<Model.Setting, Setting> _settingsConverter;
    private readonly IBidirectionalConverter<Model.SettingGroup, Model.SettingGroup> _settingsConverter;

    public MySqlSettingRepository(
        SettingsContext settingsContext,
        IBidirectionalConverter<Model.Setting, Setting> settingsConverter
    )
    {
        // Injects SettingsContext configured in ServiceConfiguration.cs into _settingsContext object
        _settingsContext = settingsContext;
        // Injects biverter that we set up into _settingsConverter object
        _settingsConverter = settingsConverter;
    }

    public async Task<Model.Setting> GetSetting(string settingId, int version, string userId, string password)
    {
        // Call .Single() because there should only be one entry with this id/version
        // and .Single() will return a single object rather than a list
        var setting = await _settingsContext.Settings.SingleAsync(
            s => s.SettingGroupId == settingId && s.Version == version
        );

        var permissions = await setting.Permissions.Single(permission => permission.UserId == UserId && permission.Password == Password); //Permissions

        return _settingsConverter.Convert(setting);
    }

    public async Task<IEnumerable<Model.Setting>> GetAllSettingVersions(string settingId)
    {
        var settings = await GetAllUnconvertedSettingVersions(settingId);

        // This could also be written as settings.Select(s => _settingsConverter.Convert(s))

        // You're allowed to abbreviate the syntax like below when you're just calling a
        // function like that on every object in a list
        return settings.Select(_settingsConverter.Convert);
    }

    public async Task<Model.Setting> CreateSetting(CreateSettingRequest request)
    {
        var setting = new Setting
        {
            SettingGroupId = request.Id,
            Version = 1,
            Input = JsonSerializer.Serialize(request.Input),
            IsCurrent = false,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // You can also use .AddAsync but awaiting both of these operations seems like overkill
        _settingsContext.Settings.Add(setting);
        await _settingsContext.SaveChangesAsync();

        // Actually retrieves the new setting from the table to confirm it's there
        return await GetSetting(request.Id, 1);
    }

    // This method will eventually be more complex. Right now we're not implementing approval process/permissions
    // so this is very similar to just creating a setting. Eventually you'll be able to do an update which creates
    // a new version OR an update which changes an existing version that hasn't been approved/used yet
    public async Task<Model.Setting> UpdateSetting(UpdateSettingRequest request)
    {
        var newVersion = (await GetLatestVersionNumber(request.Id)) + 1;
        var newSetting = new Setting
        {
            SettingGroupId = request.Id,
            Version = newVersion,
            Input = JsonSerializer.Serialize(request.Input),
            IsCurrent = false,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _settingsContext.Settings.Add(newSetting);
        await _settingsContext.SaveChangesAsync();

        return await GetSetting(request.Id, newVersion);
    }

    // Not even sure what the use-case is for this other than the user deciding not to use the service anymore
    // Still a good method to have available though, and good for demonstrating that we have full control of the data
    //
    // Also assuming for now that we'd only want to delete the full group of settings (all versions)
    public async Task<IEnumerable<Model.Setting>> DeleteSetting(string settingId)
    {
        var settings = await GetAllUnconvertedSettingVersions(settingId);
        _settingsContext.Settings.RemoveRange(settings);
        await _settingsContext.SaveChangesAsync();

        // Could probably make this void (or with async method just "Task") but would like to show user the
        // contents of what was just deleted
        return settings.Select(_settingsConverter.Convert);
    }

    private async Task<int> GetLatestVersionNumber(string settingId)
    {
        // Retrieve all settings with id
        var settings = await GetAllUnconvertedSettingVersions(settingId);

        // Pull versions off of settings list
        var versions = settings.Select(s => s.Version);
        return versions.Max();
    }

    // Not the biggest deal in this case, but breaking into its own method since this query is repeated more than once
    // Important to take any logic that is repeated more than once and break into its own method
    private async Task<List<Setting>> GetAllUnconvertedSettingVersions(string settingId)
    {
        return await _settingsContext.Settings
            .Where(s => s.SettingGroupId == settingId)
            .ToListAsync();
    }
}
