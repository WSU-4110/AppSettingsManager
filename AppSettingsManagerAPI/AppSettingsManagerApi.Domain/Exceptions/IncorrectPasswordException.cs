using System;

namespace AppSettingsManagerApi.Domain.Exceptions;

public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException(string userId)
        : base($"Incorrect password for {userId}") { }
}
