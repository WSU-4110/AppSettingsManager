using System.Collections;
using AppSettingsManagerBff.Model.Api;

namespace AppSettingsManagerBff.Domain.ApiRepositories;

public interface ISettingsRepository
{
    Task<ApiSetting> CreateSetting(CreateSettingRequest request);
    Task<ApiSetting> GetSetting(string settingId, int version);
    Task<ApiSetting> UpdateSetting(UpdateSettingRequest request);
    Task<IEnumerable<ApiSetting>> DeleteSetting(string settingId);
}
