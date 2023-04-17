using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Model;
using AppSettingsManagerApi.Model.Requests;
using SettingGroup = AppSettingsManagerApi.Model.SettingGroup;

namespace AppSettingsManagerApi.Facades;

public class SettingFacade
{
    private readonly ISettingRepository _settingRepository;
    private readonly IPermissionRepository _permissionRepository;

    public SettingFacade(
        ISettingRepository settingRepository,
        IPermissionRepository permissionRepository
    )
    {
        _settingRepository = settingRepository;
        _permissionRepository = permissionRepository;
    }

    #region Get

    public async Task<SettingGroup> GetSettingGroup(GetSettingGroupRequest request)
    {
        await _permissionRepository.CheckPermission(
            request.UserId,
            request.Password,
            request.SettingGroupId,
            PermissionLevel.Read
        );
        var settingGroup = await _settingRepository.GetSettingGroup(request.SettingGroupId);

        return settingGroup;
    }

    public async Task<IEnumerable<SettingGroup>> GetAllSettingGroupsForUser(
        GetSettingGroupRequest request
    )
    {
        await _permissionRepository.CheckPermission(
            request.UserId,
            request.Password,
            request.SettingGroupId,
            PermissionLevel.Read
        );
        var settingGroups = await _settingRepository.GetSettingGroupsByUser(request.UserId);

        return settingGroups;
    }

    public async Task<Dictionary<string, string>> GetSettings(GetSettingGroupRequest request)
    {
        await _permissionRepository.CheckPermission(
            request.UserId,
            request.Password,
            request.SettingGroupId,
            PermissionLevel.Read
        );
        
        var settings = await _settingRepository.GetSettings(request);

        return settings;
    }

    public async Task<string> GetSetting(GetSettingGroupRequest request, string variableName)
    {
        await _permissionRepository.CheckPermission(
            request.UserId,
            request.Password,
            request.SettingGroupId,
            PermissionLevel.Read
        );
        
        var settings = await _settingRepository.GetSettings(request);
        var setting = settings[variableName];

        return setting;
    }

    #endregion

    public async Task<SettingGroup> CreateSettingGroup(CreateSettingRequest request)
    {
        var settingGroup = await _settingRepository.CreateSettingGroup(
            request.SettingGroupId,
            request.UserId
        );

        var setting = _settingRepository.CreateSetting(request);

        var permissionRequest = new CreatePermissionRequest
        {
            SettingGroupId = request.SettingGroupId,
            UserId = request.UserId,
            StartingPermissionLevel = PermissionLevel.Admin
        };

        permissionRequest.SetNeedsApproval(false);

        var permission = _permissionRepository.CreatePermission(permissionRequest);

        await Task.WhenAll(setting, permission);

        return await _settingRepository.GetSettingGroup(request.SettingGroupId);
    }

    public async Task<SettingGroup> UpdateSetting(CreateSettingRequest request)
    {
        await _permissionRepository.CheckPermission(
            request.UserId,
            request.Password,
            request.SettingGroupId,
            PermissionLevel.Write
        );

        var settingGroup = await _settingRepository.CreateSetting(request);

        return settingGroup;
    }

    public async Task<SettingGroup> ChangeTargetSettingVersion(UpdateTargetSettingRequest request)
    {
        await _permissionRepository.CheckPermission(
            request.UserId,
            request.Password,
            request.SettingGroupId,
            PermissionLevel.Admin
        );
        var settingGroup = await _settingRepository.ChangeTargetSettingVersion(request);

        return settingGroup;
    }

    public async Task<SettingGroup> DeleteSettingGroup(DeleteSettingGroupRequest request)
    {
        await _permissionRepository.CheckPermission(
            request.UserId,
            request.Password,
            request.SettingGroupId,
            PermissionLevel.Admin
        );
        var settingGroup = await _settingRepository.DeleteSetting(request.SettingGroupId);

        return settingGroup;
    }

    public async Task<Permission> RequestNewSettingGroupAccess(CreatePermissionRequest request)
    {
        var permission = await _permissionRepository.CreatePermission(request);
        return permission;
    }

    public async Task<Permission> UpdateSettingGroupAccess(UpdatePermissionRequest request)
    {
        var permission = await _permissionRepository.UpdatePermission(request);
        return permission;
    }

    public async Task<Permission> PermissionRequestResponse(PermissionRequestResponse response)
    {
        await _permissionRepository.CheckPermission(
            response.ApproverId,
            response.Password,
            response.SettingGroupId,
            PermissionLevel.Admin
        );

        var permission = await _permissionRepository.PermissionRequestResponse(response);

        return permission;
    }
}
