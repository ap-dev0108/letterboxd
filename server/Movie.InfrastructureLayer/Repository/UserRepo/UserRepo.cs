using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie.Application.DTO.UserDTO;
using Movie.Application.Interface.RepoInterface.UserRepo;
using Movie.Domain.Entities;
using Movie.Infrastructure.Database;

namespace Movie.Infrastructure.Repository.UserRepo;

public class UserRepo : IUserRepo
{
    private readonly ApplicationDb _db;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager; 

    public UserRepo(ApplicationDb db, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager; 
    }

    public async Task<List<User>> GetAllUserAsync()
    {
        return await _db.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> GetUserById(string id)
    {
        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IdentityResult> RegisterAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task LoginAsync(User user, string password)
    {
        await _signInManager.CheckPasswordSignInAsync(user, password, false);
    }

    public async Task<User> CheckUserExists(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task DeleteUser(User user)
    {
        _db.Users.Remove(user);
    }
}