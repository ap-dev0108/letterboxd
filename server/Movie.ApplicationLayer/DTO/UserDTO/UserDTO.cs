using Microsoft.AspNetCore.Identity;

namespace Movie.Application.DTO.UserDTO;

public class UserDTO : IdentityUser<string>
{
    public string UserID {get; set;}
    public string? nicknames {get; set;}
    public string? UserPass {get; set;}
}