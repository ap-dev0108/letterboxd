using Microsoft.AspNetCore.Identity;
using Movie.Application.DTO.UserDTO;
using Movie.Application.DTO.UserDTO.Login;
using Movie.Domain.Entities;

namespace Movie.Application.Interface.UserInterface;

public interface IUserInterface
{
    Task<IdentityResult> RegisterUserAsync(User user, string password);
    Task<string> LoginUserAsync(LoginDTO loginDTO);
    Task<UserDTO> UserProfile(string Id);
    Task<UserDTO> DeleteUser(string id);
}