using Microsoft.AspNetCore.Identity;
using Movie.Application.DTO.UserDTO;
using Movie.Domain.Entities;

namespace Movie.Application.Interface.RepoInterface.UserRepo;

public interface IUserRepo
{
    Task<List<User>> GetAllUserAsync();
    Task<User> GetUserById(string userId);
    Task<IdentityResult> RegisterAsync(User user, string password);
    Task LoginAsync(User user, string password);
    Task<User> CheckUserExists(string email);
    Task DeleteUser(User user);
    Task VerifyUser(User user);
}