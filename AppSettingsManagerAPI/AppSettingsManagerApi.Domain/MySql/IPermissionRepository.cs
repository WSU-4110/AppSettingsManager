using AppSettingsManagerApi.Model;
using AppSettingsManagerApi.Model.Requests;
using Permission = AppSettingsManagerApi.Model.Permission;

namespace AppSettingsManagerApi.Domain.MySql;

public interface IPermissionRepository
{
    Task<Permission> GetPermission(string userId, string settingGroupId);
    Task<Permission> CreatePermission(CreatePermissionRequest request);
    Task<Permission> UpdatePermission(UpdatePermissionRequest request);
    Task<Permission> PermissionRequestResponse(PermissionRequestResponse response);
    Task<Permission> DeletePermission(string userId, string settingGroupId);

<<<<<<< HEAD
    Task CheckPermission(string userId, string password, string settingGroupId,
        PermissionLevel requiredPermissionLevel);
}
=======
    Task CheckPermission(
        string userId,
        string password,
        string settingGroupId,
        PermissionLevel requiredPermissionLevel
    );
}
>>>>>>> bfec0ed8c20ad88586f6586f1bde3aaf946741e5
