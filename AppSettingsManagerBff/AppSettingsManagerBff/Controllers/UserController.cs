using System.ComponentModel.DataAnnotations;
using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model;
using AppSettingsManagerBff.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AppSettingsManagerBff.Controllers;

[ApiController]
[Route("users")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("auth/userId/{userId}/password/{password}")]
    public async Task<bool> AuthenticateUser(
        [FromRoute][Required] string userId,
        [FromRoute][Required] string password
    )
    {
        var response = await _userRepository.AuthenticateUser(userId, password);
        return response;
    }

    [HttpPost]
    public async Task<User> CreateUser(CreateUserRequest request)
    {
        var user = await _userRepository.CreateUser(request);
        return user;
    }

    [HttpPut]
    public async Task<User> UpdateUserPassword(UpdateUserPasswordRequest request)
    {
        var user = await _userRepository.UpdateUserPassword(request);
        return user;
    }

    [HttpDelete("userId/{userId}/password/{password}")]
    public async Task<User> DeleteUser(
        [FromRoute][Required] string userId,
        [FromRoute][Required] string password
    )
    {
        var user = await _userRepository.DeleteUser(userId, password);
        return user;
    }
}