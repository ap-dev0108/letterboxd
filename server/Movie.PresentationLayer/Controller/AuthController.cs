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
    public async Task<IActionResult> GetToken()
    {
        var tokens = HttpContext.Request.Headers.Authorization.ToString();

        return Ok(new APIResponse<string>
        {
            Success = true,
            Message = "Yep you are logged in",
            Data = tokens
        });
    }

    [HttpGet("/getUsers")]
    public async Task<IActionResult> GetUsers(string userID)
    {
        var getUser = await _userService.GetUserById(userID);
        return Ok(new APIResponse<User>
        {
            Success = true,
            Message = "User Fetched",
            Data = getUser
        });
    }
}