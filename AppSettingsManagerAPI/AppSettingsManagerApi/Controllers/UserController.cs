using System.ComponentModel.DataAnnotations;
using AppSettingsManagerApi.Domain.MySql;
using Microsoft.AspNetCore.Mvc;

namespace AppSettingsManagerApi.Controllers;

[ApiController]
[Route("users")]
public class UserController
{
    private readonly IBaseUserRepository _userRepository;

    public UserController(IBaseUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("userId/{userId}")]
    public async Task<Model.BaseUser> GetUser([FromRoute] [Required] string userId)
    {
        return await _userRepository.GetUser(userId);
    }

    [HttpPost("userId/{userId}/password/{password}")]
    public async Task<Model.BaseUser> CreateUser(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string password
    )
    {
        return await _userRepository.CreateUser(userId, password);
    }
}
