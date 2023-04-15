using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.Exceptions;
using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Model;
using AppSettingsManagerApi.Model.Requests;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class MySqlPermissionRepository : IPermissionRepository
{
    private readonly SettingsContext _settingsContext;
    private readonly IBidirectionalConverter<Model.Permission, Permission> _permissionConverter;

    public MySqlPermissionRepository(
        SettingsContext settingsContext,
        IBidirectionalConverter<Model.Permission, Permission> permissionConverter
    )
    {
        _settingsContext = settingsContext;
        _permissionConverter = permissionConverter;
    }

    public async Task<Model.Permission> GetPermission(string userId, string settingGroupId)
    {
        var permission = await GetPermissionFromContext(userId, settingGroupId);

        return _permissionConverter.Convert(permission);
    }

    public async Task<Model.Permission> CreatePermission(CreatePermissionRequest request)
    {
        var permission = new Permission
        {
            UserId = request.UserId,
            SettingGroupId = request.SettingGroupId,
            CurrentPermissionLevel = request.GetNeedsApproval()
                ? PermissionLevel.None
                : request.StartingPermissionLevel,
            WaitingForApproval = request.GetNeedsApproval(),
            RequestedPermissionLevel = request.GetNeedsApproval()
                ? request.StartingPermissionLevel
                : PermissionLevel.None
        };

        _settingsContext.Permissions.Add(permission);
        await _settingsContext.SaveChangesAsync();

        return _permissionConverter.Convert(permission);
    }

    public async Task<Model.Permission> UpdatePermission(UpdatePermissionRequest request)
    {
        var permission = await GetPermissionFromContext(request.UserId, request.SettingGroupId);

        permission.RequestedPermissionLevel = request.NewPermissionLevel;
        permission.WaitingForApproval = true;
        permission.ApprovedBy = string.Empty;

        await _settingsContext.SaveChangesAsync();
        return _permissionConverter.Convert(permission);
    }

<<<<<<< HEAD
    public async Task<Model.Permission> PermissionRequestResponse(PermissionRequestResponse response)
    {
        var permission = await GetPermissionFromContext(response.UserId, response.SettingGroupId);

        permission.CurrentPermissionLevel =
            response.Approved ? permission.RequestedPermissionLevel : permission.CurrentPermissionLevel;
=======
    public async Task<Model.Permission> PermissionRequestResponse(
        PermissionRequestResponse response
    )
    {
        var permission = await GetPermissionFromContext(response.UserId, response.SettingGroupId);

        permission.CurrentPermissionLevel = response.Approved
            ? permission.RequestedPermissionLevel
            : permission.CurrentPermissionLevel;
>>>>>>> bfec0ed8c20ad88586f6586f1bde3aaf946741e5
        permission.ApprovedBy = response.Approved ? response.ApproverId : string.Empty;
        permission.RequestedPermissionLevel = PermissionLevel.None;
        permission.WaitingForApproval = false;

        await _settingsContext.SaveChangesAsync();

        return _permissionConverter.Convert(permission);
    }

    public async Task<Model.Permission> DeletePermission(string userId, string settingGroupId)
    {
        var permission = await GetPermissionFromContext(userId, settingGroupId);

        _settingsContext.Permissions.Remove(permission);

        return _permissionConverter.Convert(permission);
    }

<<<<<<< HEAD
    public async Task CheckPermission(string userId, string password, string settingGroupId, PermissionLevel requiredPermissionLevel)
=======
    public async Task CheckPermission(
        string userId,
        string password,
        string settingGroupId,
        PermissionLevel requiredPermissionLevel
    )
>>>>>>> bfec0ed8c20ad88586f6586f1bde3aaf946741e5
    {
        if (string.IsNullOrEmpty(settingGroupId))
        {
            var user = await _settingsContext.Users.SingleAsync(u => u.Id == userId);
<<<<<<< HEAD
            
=======

>>>>>>> bfec0ed8c20ad88586f6586f1bde3aaf946741e5
            if (user.Password != password)
            {
                throw new IncorrectPasswordException(userId);
            }
        }
        else
        {
            var permission = await GetPermissionFromContext(userId, settingGroupId);
<<<<<<< HEAD
            
=======

>>>>>>> bfec0ed8c20ad88586f6586f1bde3aaf946741e5
            if (permission.User.Password != password)
            {
                throw new IncorrectPasswordException(userId);
            }

            if (permission.CurrentPermissionLevel < requiredPermissionLevel)
            {
                throw new InsufficientPermissionException(userId, settingGroupId);
            }
        }
    }

    private async Task<Permission> GetPermissionFromContext(string userId, string settingGroupId) =>
<<<<<<< HEAD
        await _settingsContext.Permissions.Include(p => p.User).SingleAsync(
            p => p.UserId == userId && p.SettingGroupId == settingGroupId
        );
}
=======
        await _settingsContext.Permissions
            .Include(p => p.User)
            .SingleAsync(p => p.UserId == userId && p.SettingGroupId == settingGroupId);
}
>>>>>>> bfec0ed8c20ad88586f6586f1bde3aaf946741e5
