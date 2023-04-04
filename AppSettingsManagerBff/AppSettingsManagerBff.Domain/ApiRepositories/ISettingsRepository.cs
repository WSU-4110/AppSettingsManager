using AppSettingsManagerBff.Model;
using AppSettingsManagerBff.Model.Requests;

namespace AppSettingsManagerBff.Domain.ApiRepositories;

public interface ISettingsRepository
{
    Task<SettingGroup> GetSettingGroup(string userId, string password, string settingGroupId);
    Task<IEnumerable<SettingGroup>> GetSettingGroupsForUser(string userId, string password);
    Task<SettingGroup> CreateSettingGroup(CreateSettingRequest request);
    Task<SettingGroup> UpdateSetting(CreateSettingRequest request);
    Task<SettingGroup> ChangeTargetSettingVersion(UpdateTargetSettingRequest request);
    Task<SettingGroup> DeleteSettingGroup(string userId, string password, string settingGroupId);
}