using AppSettingsManagerApi.Model.Requests;

namespace AppSettingsManagerApi.Domain.MySql;

public interface ISettingRepository
{
    Task<Model.SettingVersion> GetSetting(string settingId, int version);
    Task<IEnumerable<Model.SettingVersion>> GetAllSettingVersions(string settingId);
    Task<Model.SettingVersion> CreateSetting(CreateSettingRequest request);
    Task<Model.SettingVersion> UpdateSetting(UpdateSettingRequest request);
    Task<IEnumerable<Model.SettingVersion>> DeleteSetting(string settingId);
}
