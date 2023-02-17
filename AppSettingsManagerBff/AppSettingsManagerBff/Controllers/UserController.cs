using System.ComponentModel.DataAnnotations;
using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model.Api;
using Microsoft.AspNetCore.Mvc;

namespace AppSettingsManagerBff.Controllers;

[ApiController]
[Route("users")]
public class UserController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("userId/{userId}")]
    public async Task<ApiBaseUser> GetUser([FromRoute] [Required] string userId)
    {
        var user = await _userRepository.GetUser(userId);
        return user;
    }
}