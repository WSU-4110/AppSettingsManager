using AppSettingsManagerApi.Model.Requests;

namespace AppSettingsManagerApi.Domain.MySql;

public interface ISettingRepository
{
    Task<Model.Setting> GetSetting(string settingId, int version);
    Task<IEnumerable<Model.Setting>> GetAllSettingVersions(string settingId);
    Task<Model.Setting> CreateSetting(CreateSettingRequest request);
    Task<Model.Setting> UpdateSetting(UpdateSettingRequest request);
    Task<IEnumerable<Model.Setting>> DeleteSetting(string settingId);
}
