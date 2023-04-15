using System.ComponentModel.DataAnnotations;
using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Model.Requests;
using Microsoft.AspNetCore.Mvc;
using User = AppSettingsManagerApi.Model.User;

namespace AppSettingsManagerApi.Controllers;

[ApiController]
[Route("users")]
public class UserController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("auth/userId/{userId}/password/{password}")]
    public async Task<bool> AuthenticateUser(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string password
    )
    {
        return await _userRepository.AuthenticateUser(userId, password);
    }

    [HttpPost]
    public async Task<User> CreateUser([FromBody] [Required] CreateUserRequest request)
    {
        var user = await _userRepository.CreateUser(request);
        return user;
    }

    [HttpPut]
    public async Task<User> UpdateUser([FromBody] [Required] UpdateUserPasswordRequest request)
    {
        var user = await _userRepository.UpdateUserPassword(request);
        return user;
    }

    [HttpDelete("userId/{userId}/password/{password}")]
    public async Task<User> DeleteUser(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string password
    )
    {
        var request = new DeleteUserRequest { UserId = userId, Password = password };

        var user = await _userRepository.DeleteUser(request);
        return user;
    }
}
