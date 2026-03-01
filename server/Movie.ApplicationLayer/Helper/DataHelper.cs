using System.Security.Claims;

namespace Movie.Application.Helper.Data;

public class DataHelper
{
    private readonly IHttpContextAccessor _context;

    public DataHelper(IHttpContextAccessor context)
    {
        _context = context;
    }

    public (string userId, string role) GetTokenDetails()
    {
        try
        {
            var userID = _context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roles = _context.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            return new (userID, roles);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}