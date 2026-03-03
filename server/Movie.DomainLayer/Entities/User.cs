using Microsoft.AspNetCore.Identity;
using Movie.Domain.Entities.Reviews;

namespace Movie.Domain.Entities;

public class User : IdentityUser
{
    public string? Nickname {get; set;}
    public ICollection<Ratings> UserRatings {get; set;} = new List<Ratings>();
    public ICollection<Review> UserReviews{get; set;} = new List<Review>();
}