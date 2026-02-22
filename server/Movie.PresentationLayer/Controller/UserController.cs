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
    public async Task<APIResponse<List<User>>> GetAllUsers()
    {
        try
        {
            if (User.IsInRole("User"))
            {
                return new ()
                {
                    Success = false,
                    Message = "You are not allowed to use this method"  
                };
            }
            var getUsers = await _userServices.GetAllUserAsync();
            if (getUsers.Count == 0) return new ()
            {
                Success = true,
                Message = "No Content Found",
                Data = null
            };

            return new ()
            {
                Success = true,
                Message = "Users fetched Successfully",
                Data = getUsers
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }

    [AllowAnonymous]
    [HttpPost("/register")]
    public async Task<APIResponse<UserDTO>> AddUser([FromBody] UserDTO userDTO)
    {
        try
        {
            var existingUser = await _userServices.CheckUserExists(userDTO.Email);

            if(existingUser != null) return new ()
            {
                Success = false,
                Message = "User Already Exists",
                Data = new UserDTO
                {
                    UserName = existingUser.UserName
                }
            };

            var user = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email
            };

            var addUser = await _userServices.RegisterUserAsync(user, userDTO.UserPass);

            if(!addUser.Succeeded) return new ()
            {
                Success = false,
                Message = "Cannot register User",
                Data = null
            };

            return new ()
            {
                Success = true,
                Message = "User Registered",
                Data = new UserDTO
                {
                    UserName = user.UserName
                }
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = $"Exception: {ex.Message}",
                Data = null
            };
        }
    }

    [AllowAnonymous]
    [HttpPost("/login")]
    public async Task<APIResponse<string>> LoginUser([FromBody] LoginDTO loginDTO)
    {
        try
        {
            var checkUserExists = await _userServices.CheckUserExists(loginDTO.usermail);
            if(checkUserExists == null) return new()
            {
                Success = false,
                Message = "Email not found",
                Data = null  
            };

            var loginUser = await _userServices.LoginUserAsync(loginDTO);
            
            return new ()
            {
                Success = true,
                Message = "Login Success",
                Data = loginUser
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }

    [Authorize]
    [HttpGet("user-profile")]
    public async Task<APIResponse<UserDTO>> VerifyUser()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            Console.WriteLine($"User ID: {userId}");
            if (string.IsNullOrEmpty(userId))
            {
                return new ()
                {
                    Success = false,
                    Message = "Invalid or missing token. Please login again.",
                    Data = null
                };
            }

            var profile = await _userServices.UserProfile(userId);

            if(profile == null) return new ()
            {
                Success = false,
                Message = "User profile not found",
                Data = null
            };

            return new()
            {
                Success = true,
                Message = "Got your profile",
                Data = profile
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = $"System Exception: {ex.Message}"
            };
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("delete-user")]
    public async Task<APIResponse<UserDTO>> DeleteUser(string userID)
    {
        try
        {
            var users = await _userServices.DeleteUser(userID);

            return new ()
            {
                Success = true,
                Message = "User has been deleted",
                Data = users
            };

        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = $"Controller Thrown Exception: {ex.Message}",
                Data = null
            };
        }
    }
}