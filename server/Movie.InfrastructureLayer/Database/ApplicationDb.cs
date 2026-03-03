using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie.Domain.Entities;
using Movie.Domain.Entities.Reviews;

namespace Movie.Infrastructure.Database;

public class ApplicationDb : IdentityDbContext<User>
{
    public ApplicationDb(DbContextOptions<ApplicationDb> options) : base(options)
    {
        
    }
    public DbSet<Film> Films {get; set;}
    public DbSet<Genre> Genres {get; set;}
    public DbSet<Ratings> Ratings {get; set;}
    public DbSet<Review> Reviews {get; set;}
}