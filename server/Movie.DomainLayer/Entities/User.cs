using Microsoft.AspNetCore.Identity;

namespace Movie.Domain.Entities;

public class User : IdentityUser
{
    public string? Nickname {get; set;}
    public ICollection<Ratings> UserRatings {get; set;} = new List<Ratings>();
}