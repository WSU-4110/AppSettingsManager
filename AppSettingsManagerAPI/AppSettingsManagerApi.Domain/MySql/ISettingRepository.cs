using AppSettingsManagerApi.Model;
using AppSettingsManagerApi.Model.Requests;

namespace AppSettingsManagerApi.Domain.MySql;

public interface ISettingRepository
{
    Task<SettingGroup> GetSettingGroup(string settingGroupId);
    Task<IEnumerable<SettingGroup>> GetSettingGroupsByUser(string userId);
    Task<Dictionary<string, string>> GetSettings(GetSettingGroupRequest request);
    Task<SettingGroup> CreateSetting(CreateSettingRequest request);
    Task<SettingGroup> CreateSettingGroup(string settingGroupId, string createdBy);
    Task<SettingGroup> ChangeTargetSettingVersion(UpdateTargetSettingRequest request);
    Task<SettingGroup> DeleteSetting(string settingGroupId);
}
