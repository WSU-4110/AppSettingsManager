using System.ComponentModel.DataAnnotations;
using AppSettingsManagerApi.Domain.MySql;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("userId/{userId}")]
    public async Task<Model.User> GetUser([FromRoute] [Required] string userId)
    {
        return await _userRepository.GetUser(userId);
    }

    [HttpPost("userId/{userId}/password/{password}/email/{email}")]
    public async Task<Model.User> CreateUser(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string password,
        [FromRoute] [Required] string email
    )
    {
        return await _userRepository.CreateUser(userId, password, email);
    }

    [HttpPut("userId/{userId}/password/{newPassword}")]
    public async Task<Model.User> UpdateUser(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string newPassword
    )
    {
        return await _userRepository.UpdateUser(userId, newPassword);
    }

    [HttpDelete("delete/userId/{userId}")]
    public async Task<Model.User> DeleteUser([FromRoute] [Required] string userId)
    {
        return await _userRepository.DeleteUser(userId);
    }
}
