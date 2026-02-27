using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Movie.Application.Service.UserService;
using Movie.Domain.Entities;

namespace Movie.Presentation.Controller.AuthController;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("/checkLogin")]
    public async Task<APIResponse<IActionResult>> GetToken()
    {
        var tokens = HttpContext.Request.Headers.Authorization.ToString();
        if(string.IsNullOrEmpty(tokens))
        {
            return new ()
            {
                Success = false,
                Message = "Token not provided. Please login again"
            };
        }

        return new ()
        {
            Success = true,
            Message = "Yep you are logged in"
        };
    }

    [HttpGet("/getUsers")]
    public async Task<APIResponse<User>> GetUsers(string userID)
    {
        try
        {
            var getUser = await _userService.GetUserById(userID);
            return new ()
            {
                Success = true,
                Message = "User Fetched",
                Data = getUser
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}