using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Movie.Application.DTO.UserDTO;
using Movie.Application.DTO.UserDTO.Login;
using Movie.Application.Helper.Data;
using Movie.Application.Interface.RepoInterface.UserRepo;
using Movie.Application.Interface.TokenInterface;
using Movie.Application.Interface.UserInterface;
using Movie.Domain.Entities;

namespace Movie.Application.Service.UserService;

public class UserService : IUserInterface
{
    private readonly IUserRepo _userRepo;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _token;

    public UserService(IUserRepo userRepo, UserManager<User> userManager, SignInManager<User> signInManager, ITokenService token)
    {
        _userRepo = userRepo;
        _userManager = userManager;
        _signInManager = signInManager;
        _token = token;
    }

    public async Task<List<User>> GetAllUserAsync()
    {
        var getUsers = await _userRepo.GetAllUserAsync() ??
            throw new KeyNotFoundException("User value is null");

        return getUsers;
    }

    public async Task<User> GetUserById(string userId)
    {
        var getUsers = await _userRepo.GetUserById(userId) ?? 
            throw new KeyNotFoundException("User with this ID not found");

        return getUsers;
    }
    public async Task<IdentityResult> RegisterUserAsync(User user, string password)
    {
        try
        {
            var checkUserExists = await _userManager.GetEmailAsync(user) ?? 
                throw new KeyNotFoundException("User with this mail cannot be found");

            var registerUser = await _userRepo.RegisterAsync(user, password);

            if(!registerUser.Succeeded) 
                throw new UnauthorizedAccessException("Invalid form of email or password");

            await _userManager.AddToRoleAsync(user, "User");
            await _token.GenerateToken(user);
            
            return registerUser;
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception: {ex.Message}");
        }
    }
    public async Task<string> LoginUserAsync(LoginDTO loginDTO)
    {
        try
        {
            // check if mail exists
            var user = await _userManager.FindByEmailAsync(loginDTO.usermail) ?? 
                throw new UnauthorizedAccessException("User with this mail cannot be found");

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.userpassword, false);

            if(!checkPassword.Succeeded) 
                throw new UnauthorizedAccessException("Invalid email or password");

            return await _token.GenerateToken(user);
        }

        catch (Exception ex)
        {
            throw new Exception($"Service Exception: {ex.Message}");
        }
    }

    public async Task<UserDTO> UserProfile(string userId)
    {
        try
        {
            // if (string.IsNullOrEmpty(userId))
            //     throw new KeyNotFoundException("User with this ID cannot be found");
            var userData = await _userRepo.GetUserById(userId) ?? 
                throw new KeyNotFoundException("User with this ID cannot be found");

            return new UserDTO
            {
                UserName = userData.UserName,
                Email = userData.Email
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserDTO> DeleteUser(string userID)
    {
        try
        {
            var checkUser = await _userRepo.GetUserById(userID) ??
                throw new KeyNotFoundException($"UserID with ID '{userID}' could not be found");

            await _userRepo.DeleteUser(checkUser);
            var userDTO = new UserDTO
            {
                UserName = checkUser.UserName
            };

            return userDTO;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}