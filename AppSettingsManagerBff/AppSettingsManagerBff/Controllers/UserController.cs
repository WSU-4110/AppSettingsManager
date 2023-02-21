using System.ComponentModel.DataAnnotations;
using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model.Api;
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

    [HttpGet("userId/{userId}")]
    public async Task<ApiBaseUser> GetUser([FromRoute] [Required] string userId)
    {
        var user = await _userRepository.GetUser(userId);
        return user;
    }

    [HttpPost("userId/{userId}/password/{password}/email/{email}")]
    public async Task<ApiBaseUser> CreateUser([FromRoute] [Required] string)

    [HttpPut]
    public async Task<ApiBaseUser> UpdateUser([FromRoute][Required] UpdateUserRequest request)
    {
        return await _userRepository.UpdateUser(request);
    }

    [HttpDelete("delete/userId/{userId}")]
    public async Task<ApiBaseUser> DeleteUser([FromRoute][Required] string userId)
    {
        return await _userRepository.DeleteUser(userId);
    }
}
