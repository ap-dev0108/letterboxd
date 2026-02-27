using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Application.DTO.UserDTO;
using Movie.Application.DTO.UserDTO.Login;
using Movie.Application.Service.UserService;
using Movie.Domain.Entities;

namespace Movie.Presentation.Controller.UserControllers;

[ApiController]
[Route("/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userServices;

    public UserController(UserService userService)
    {
        _userServices = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("/getAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        if (User.IsInRole("User"))
        {
            return Unauthorized(new 
            {
                Success = false,
                Message = "You are not allowed to use this method"  
            });
        }
        var getUsers = await _userServices.GetAllUserAsync();
        if (!getUsers.Any()) return NoContent();

        return Ok(new APIResponse<List<User>>
        {
            Success = true,
            Message = "Users fetched Successfully",
            Data = getUsers
        });
    }

    [AllowAnonymous]
    [HttpPost("/register")]
    public async Task<IActionResult> AddUser([FromBody] UserDTO userDTO)
    {
        var user = new User
        {
            UserName = userDTO.UserName,
            Email = userDTO.Email
        };

        await _userServices.RegisterUserAsync(user, userDTO.UserPass);

        return Ok(new APIResponse<UserDTO>
        {
            Success = true,
            Message = "User Registered",
            Data = new UserDTO
            {
                UserName = user.UserName
            }
        });
    }

    [AllowAnonymous]
    [HttpPost("/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDTO loginDTO)
    {
        var loginUser = await _userServices.LoginUserAsync(loginDTO);
        
        return Ok(new APIResponse<string>
        {
            Success = true,
            Message = "Login Success",
            Data = loginUser
        });
    }

    [Authorize]
    [HttpGet("user-profile")]
    public async Task<IActionResult> VerifyUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        var profile = await _userServices.UserProfile(userId);

        if(profile == null) return NoContent();

        return Ok(new APIResponse<UserDTO>
        {
            Success = true,
            Message = "Got your profile",
            Data = profile
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("delete-user")]
    public async Task<IActionResult> DeleteUser(string userID)
    {
        var users = await _userServices.DeleteUser(userID);

        return Ok(new APIResponse<UserDTO>
        {
            Success = true,
            Message = "User has been deleted",
            Data = users
        });
    }
}