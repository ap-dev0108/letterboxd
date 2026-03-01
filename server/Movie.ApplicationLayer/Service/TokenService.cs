// TokenService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.IdentityModel.Tokens;
using Movie.Application.Interface.TokenInterface;
using Movie.Domain.Entities;
using Movie.Domain.Entities.Token;
using Movie.Infrastructure.Database;

namespace Movie.Application.Service.TokenService;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public TokenService(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    private const string Secret = "940e7459e7860742e9a9495da3c64e9065d703a6efc8c56394c9c013995affe0";
    public async Task<string> GenerateToken(User user)
    {
        var userRole = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach(var role in userRole)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiryMinutes = _configuration.GetValue("TokenSettings:ExpiryMinutes", 60);
        var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var token = new JwtSecurityToken(
            issuer: "MovieAPI",
            audience: "MovieAPI",
            claims: claims,
            expires: expires,
            signingCredentials: creds
            
        );

        if(string.IsNullOrEmpty(Secret) || string.IsNullOrEmpty(token.Issuer))
        {
            throw new KeyNotFoundException("Env was not configured");
        }

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
