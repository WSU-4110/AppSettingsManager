using System;

namespace AppSettingsManagerApi.Domain.Exceptions;

public class InsufficientPermissionException : Exception
{
    public InsufficientPermissionException(string userId, string settingGroupId)
        : base($"{userId} has insufficient permissions to perform that action on {settingGroupId}")
    { }
}
