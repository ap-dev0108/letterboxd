using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Movie.Application.DTO.UserDTO;
using Movie.Application.DTO.UserDTO.Login;
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
        try
        {
            var getUsers = await _userRepo.GetAllUserAsync();
            return getUsers;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> GetUserById(string userId)
    {
        try
        {
            var getUsers = await _userRepo.GetUserById(userId);
            if (getUsers == null) return null;

            return getUsers;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> CheckUserExists(string email)
    {
        try
        {
            var userExists = await _userManager.FindByEmailAsync(email);
            if (userExists == null) return null;

            return userExists;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IdentityResult> RegisterUserAsync(User user, string password)
    {
        try
        {
            var checkUserExists = await _userRepo.CheckUserExists(user.Email);

            if (checkUserExists != null) return IdentityResult.Failed(
                new IdentityError
                {
                    Description = "User already exists"
                }
            );

            var registerUser = await _userRepo.RegisterAsync(user, password);

            if(registerUser.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
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
            var user = await _userManager.FindByEmailAsync(loginDTO.usermail) ?? throw new Exception("User with this mail cannot be found");

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.userpassword, false);
            if(checkPassword.IsLockedOut) throw new Exception("User is locked out");
            if(checkPassword.IsNotAllowed) throw new Exception("User Email is not verified");
            if (!checkPassword.Succeeded) throw new Exception("Passwords Don't match");

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
            if (string.IsNullOrEmpty(userId))
                return null;

            var userData = await _userRepo.GetUserById(userId);
            if (userData == null)
                return null;

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
            var checkUser = await _userRepo.GetUserById(userID);

            if (checkUser == null) return null;

            var user = _userRepo.DeleteUser(checkUser);
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